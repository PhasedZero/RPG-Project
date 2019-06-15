using System;
using RPG.Combat;
using RPG.Control;
using RPG.Movement;
using UnityEngine;

namespace RPG.Core {
    public class AiController : MonoBehaviour {
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspicionTime = 3f;
        [SerializeField] private PatrolPath patrol;
        [SerializeField] private float waypointTolerance = 1f;
        [SerializeField] private float waypointDwellTime = 2f;
        [Range(0,1)]
        [SerializeField] private float patrolSpeedFraction = 0.2f;

        // Cached GOs
        private GameObject player;
        private Fighter fighter;
        private Health health;
        private Mover mover;

        // State vars
        private State state = State.Guarding;
        private Vector3 guardPosition;
        private float timeSinceLastSawPlayer;
        private int currentWaypointIndex = 0;
        private float timeSinceArrivedAtWaypoint;

        private enum State {
            Guarding,
            Attacking,
            Suspicious
        }

        private void OnValidate() {
            guardPosition = transform.position;
        }

        private void Awake() {
            mover = GetComponent<Mover>();
            health = GetComponent<Health>();
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
        }

        private void Update() {
            if (health.IsDead) return;
            switch (state) {
                case State.Guarding:
                    GuardBehavior();
                    break;
                case State.Attacking:
                    AttackBehavior();
                    break;
                case State.Suspicious:
                    SuspicionBehavior();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void PatrolBehavior() {
            if (AtWaypoint()) {
                timeSinceArrivedAtWaypoint = 0;
                CycleWaypoint();
            }

            if (timeSinceArrivedAtWaypoint > waypointDwellTime) {
                mover.StartMoveAction(GetCurrentWaypoint(), patrolSpeedFraction);
            }

            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private bool AtWaypoint() {
            var distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }
        
        private void CycleWaypoint() {
            currentWaypointIndex = patrol.GetNextIndex(currentWaypointIndex);
        }
        
        private Vector3 GetCurrentWaypoint() {
            return patrol.GetPosition(currentWaypointIndex);
        }

        private void StartGuarding() {
            state = State.Guarding;
            mover.StartMoveAction(guardPosition, patrolSpeedFraction);
        }

        private void GuardBehavior() {
            if (CanAttack()) {
                StartAttacking();
            }
            else if (patrol) {
                PatrolBehavior();
            }
            else if(!IsAtPost()){
                mover.StartMoveAction(guardPosition, patrolSpeedFraction);
            }
        }
        
        private bool IsAtPost() {
            return transform.position == guardPosition;
        }

        private void StartAttacking() {
            state = State.Attacking;
            fighter.Attack(player);
        }

        private void AttackBehavior() {
            if (!InChaseRange()) {
                StartSuspicion();
            }
        }

        private bool CanAttack() {
            return InChaseRange() && fighter.CanAttack(player);
        }

        private bool InChaseRange() {
            return Vector3.Distance(transform.position, player.transform.position) <= chaseDistance;
        }

        private void StartSuspicion() {
            fighter.Cancel();
            timeSinceLastSawPlayer = 0f;
            state = State.Suspicious;
        }

        private void SuspicionBehavior() {
            if (CanAttack()) {
                StartAttacking();
            }
            else if (timeSinceLastSawPlayer < suspicionTime) {
                timeSinceLastSawPlayer += Time.deltaTime;
            }
            else {
                StartGuarding();
            }
        }

        // Editor functions Called by Unity
        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
