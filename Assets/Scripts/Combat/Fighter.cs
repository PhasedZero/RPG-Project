using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour, IAction {
        [SerializeField] private float weaponRange = 2f;

        private Transform target;
        private Mover mover;
        private ActionScheduler actionScheduler;

        private void Start() {
            actionScheduler = GetComponent<ActionScheduler>();
            mover = GetComponent<Mover>();
        }

        private void Update() {
            if (!target) return;

            if (!InRange()) {
                mover.MoveTo(target.position);
            }
            else {
                mover.Cancel();
            }
        }

        public void Cancel() {
            target = null;
        }

        private bool InRange() {
            return Vector3.Distance(transform.position, target.position) <= weaponRange;
        }

        public void Attack(CombatTarget combatTarget) {
            actionScheduler.StartAction(this);
            target = combatTarget.transform;
        }
    }
}
