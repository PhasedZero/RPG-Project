using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour, IAction {
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttacks = 1f;

        private Transform target;
        private Mover mover;
        private ActionScheduler actionScheduler;
        private Animator animator;

        private float timeSinceLastAttack;

        private void Start() {
            animator = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
            mover = GetComponent<Mover>();
        }

        private void Update() {
            timeSinceLastAttack += Time.deltaTime;
            if (!target) return;

            if (!InRange()) {
                mover.MoveTo(target.position);
            }
            else {
                mover.Cancel();
                AttackBehavior();
            }
        }
        private void AttackBehavior() {
            if (timeBetweenAttacks <= timeSinceLastAttack) {
                timeSinceLastAttack = 0f;
                animator.SetTrigger("attack");
            }
        }

        public void Cancel() {
            target = null;
            animator.ResetTrigger("attack");
        }

        private bool InRange() {
            return Vector3.Distance(transform.position, target.position) <= weaponRange;
        }

        public void Attack(CombatTarget combatTarget) {
            actionScheduler.StartAction(this);
            target = combatTarget.transform;
        }

        // Animation Event
        void Hit() {

        }
    }
}
