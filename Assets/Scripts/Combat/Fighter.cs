using RPG.Movement;
using UnityEngine;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour {
        [SerializeField] private float weaponRange = 2f;
        
        private Transform target;
        private Mover mover;

        private void Start() {
            mover = GetComponent<Mover>();
        }
        private void Update() {
            if (target && !InRange()) {
                mover.MoveTo(target.position);
            }
            else {
                mover.Stop();
            }
        }

        private bool InRange() {
            return Vector3.Distance(transform.position, target.position) <= weaponRange;
        }
        
        public void Attack(CombatTarget combatTarget) {
            target = combatTarget.transform;
            print("Have at thee!");
        }
    }
}
