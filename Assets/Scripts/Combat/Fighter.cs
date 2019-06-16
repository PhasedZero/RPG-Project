using System;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour, IAction {
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private Transform handTransform = null;
        [SerializeField] private Weapon weapon = null;

        private Health target;
        private Mover mover;
        private ActionScheduler actionScheduler;
        private Animator animator;

        private float timeSinceLastAttack;

        private void Awake() {
            animator = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
            mover = GetComponent<Mover>();
        }

        private void Start() {
            SpawnWeapon();
        }

        private void SpawnWeapon() {
            if (weapon == null) return;
            weapon.Spawn(handTransform, animator);
        }

        private void Update() {
            timeSinceLastAttack += Time.deltaTime;
            if (!target) return;
            if (target.IsDead) {
                Cancel();
                return;
            }

            if (!InRange()) {
                mover.MoveTo(target.transform.position, 1);
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
            target.TakeDamage(weapon.GetDamage());
        }

        public void Cancel() {
            var layer = animator.GetLayerIndex("Base Layer");

            if (animator.GetCurrentAnimatorStateInfo(layer).IsName("Base Layer.Attack") &&
                !animator.IsInTransition(layer)) {
                animator.SetTrigger("stopAttack");
            }

            mover.Cancel();
            target = null;
        }

        private bool InRange() {
            return Vector3.Distance(transform.position, target.transform.position) <= weapon.GetRange();
        }

        public bool CanAttack(GameObject combatTarget) {
            if (!combatTarget) return false;
            var targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest && !targetToTest.IsDead;
        }

        public void Attack(GameObject combatTarget) {
            actionScheduler.StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }
    }
}