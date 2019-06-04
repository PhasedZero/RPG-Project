using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour, IAction {
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttacks = 1f;

        [SerializeField] private float weaponDamage = 5f;

        private Health target;
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
            if (!target || target.IsDead) return;

            if (!InRange()) {
                mover.MoveTo(target.transform.position);
            }
            else {
                mover.Cancel();
                AttackBehavior();
            }
        }

        private void AttackBehavior() {
            transform.LookAt(target.transform);
            if (timeBetweenAttacks <= timeSinceLastAttack) {
                timeSinceLastAttack = 0f;
                animator.SetTrigger("attack");
                // Triggers in Hit() event
            }
        }

        // Animation Event
        void Hit() {
            if (!target) return;
            target.TakeDamage(weaponDamage);
        }

        public void Cancel() {
            var layer = animator.GetLayerIndex("Base Layer");
            
            if (animator.GetCurrentAnimatorStateInfo(layer).IsName("Base Layer.Attack")) {
                animator.SetTrigger("stopAttack");
            }
            
            target = null;
        }

        private bool InRange() {
            return Vector3.Distance(transform.position, target.transform.position) <= weaponRange;
        }

        public bool CanAttack(CombatTarget combatTarget) {
            if (!combatTarget) return false;
            var targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest && !targetToTest.IsDead;
        }

        public void Attack(CombatTarget combatTarget) {
            actionScheduler.StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

    }
}
