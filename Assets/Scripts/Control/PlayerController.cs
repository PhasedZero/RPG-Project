using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control {
    public class PlayerController : MonoBehaviour {

        private Camera mainCamera;
        private Mover mover;
        private Fighter fighter;
        private Health health;

        private void Start() {
            health = GetComponent<Health>();
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            mainCamera = Camera.main;
        }

        private void Update() {
            if (health.IsDead) return;
            if (Input.GetKeyDown("left shift")) {
                mover.ToggleSprint();
            }

            if (InteractWithCombat()) return;

            if (InteractWithMovement()) return;
        }

        private bool InteractWithCombat() {
            var hits = Physics.RaycastAll(GetRay());
            
            foreach (var hit in hits) {
                var target = hit.transform.GetComponent<CombatTarget>();
                
                if (!target) continue;
                if (!fighter.CanAttack(target.gameObject)) continue;

                if (Input.GetMouseButton(0)) {
                    fighter.Attack(target.gameObject);
                }

                return true;
            }

            return false;
        }

        private bool InteractWithMovement() {
            var hasHit = Physics.Raycast(GetRay(), out var hit);
            if (!hasHit) return false;

            if (Input.GetMouseButton(0)) {
                mover.StartMoveAction(hit.point);
            }

            return true;

        }

        private Ray GetRay() {
            return mainCamera.ScreenPointToRay(Input.mousePosition);
        }
    }
}
