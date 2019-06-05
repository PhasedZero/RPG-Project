using System;
using RPG.Combat;
using RPG.Movement;
using UnityEngine;

namespace RPG.Core {
    public class AiController : MonoBehaviour {
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float guardDelay = 3f;

        private GameObject player;
        private Fighter fighter;

        private Health health;
        private Mover mover;
        private Vector3 guardPosition;
        private float timeSinceLastSawPlayer;

        private enum State {
            Attacking,
            Suspicious,
            Guarding
        }
        
        private State state = State.Guarding;

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
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void StartGuarding() {
            state = State.Guarding;
            mover.StartMoveAction(guardPosition);
        }

        private void GuardBehavior() {
            if (CanAttack()) {
                StartAttacking();
            }
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
