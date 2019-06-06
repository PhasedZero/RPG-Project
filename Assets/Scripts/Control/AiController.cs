using System;
using RPG.Combat;
using RPG.Control;
using RPG.Movement;
using UnityEngine;

namespace RPG.Core {
    public class AiController : MonoBehaviour {
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float guardDelay = 3f;
        [SerializeField] private PatrolPath patrol;
        [SerializeField] private float waypointTolerance = 1f;

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

        private enum State {
            Attacking,
            Suspicious,
            Guarding,
            Patrolling
        }

        private void Start() {
            mover = GetComponent<Mover>();
            health = GetComponent<Health>();
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");

            guardPosition = transform.position;
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
                case State.Patrolling:
                    PatrolBehavior();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void StartPatrolling() {
            state = State.Patrolling;
            mover.StartMoveAction(guardPosition);
            mover.IsSprinting = false;
        }

        private void PatrolBehavior() {
            if (CanAttack()) {
                StartAttacking();
                return;
            }
            if (AtWaypoint()) {
                CycleWaypoint();
            }
            mover.StartMoveAction(GetCurrentWaypoint());
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
            mover.StartMoveAction(guardPosition);
        }

        private void GuardBehavior() {
            if (CanAttack()) {
                StartAttacking();
            }
            else if (patrol) {
                StartPatrolling();
            }
        }

        private void StartAttacking() {
            mover.IsSprinting = true;
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
            else if (timeSinceLastSawPlayer < guardDelay) {
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
