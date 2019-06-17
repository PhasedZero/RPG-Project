using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour, IAction {
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;
        [SerializeField] private Weapon defaultWeapon = null;
        [SerializeField] private Projectile ammo;

        private Health target;
        private Mover mover;
        private ActionScheduler actionScheduler;
        private Animator animator;

        private float timeSinceLastAttack;
        private Weapon currentWeapon = null;
        private GameObject equippedWeapon;

        private void Awake() {
            animator = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
            mover = GetComponent<Mover>();
        }

        private void Start() {
            EquipWeapon(defaultWeapon);
        }

        public void EquipWeapon(Weapon weapon) {
            if (equippedWeapon) {
                Destroy(equippedWeapon);
            }

            currentWeapon = weapon;
            equippedWeapon = weapon.Spawn(rightHandTransform, leftHandTransform, animator);
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
        private void Hit() {
            if (!target || target.IsDead) return;
            if (currentWeapon.HasProjectile()) {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target);
            }
            else {
                target.TakeDamage(currentWeapon.GetDamage());
            }
        }

        // Animation Event
        public void Shoot() {
            Hit();
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
            return Vector3.Distance(transform.position, target.transform.position) <= currentWeapon.GetRange();
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

//        public object CaptureState()
//        {
//            return JsonUtility.ToJson(currentWeapon);
//        }
// 
//        public void RestoreState(object state)
//        {
//            Weapon restored = ScriptableObject.CreateInstance<Weapon>();
//            JsonUtility.FromJsonOverwrite((string)state, restored);
//            EquipWeapon(restored);
//        }
//        + destroying weapon (GameObject only) that character was holding...
    }
}