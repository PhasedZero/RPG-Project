using RPG.Combat;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control {
    public class PlayerController : MonoBehaviour {
        
        private Camera mainCamera;
        private Mover mover;
        private Fighter fighter;

        private void Start() {
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            mainCamera = Camera.main;
        }

        private void Update() {
            ProcessControls();
        }

        private void InteractWithCombat() {
            RaycastHit[] hits = Physics.RaycastAll(GetRay());
            foreach (var hit in hits) {
                var target = hit.transform.GetComponent<CombatTarget>();
                
                if (!target) continue;
                fighter.Attack(target);
            }
        }

        private void ProcessControls() {
            if (Input.GetMouseButtonDown(0)) {
                InteractWithCombat();
            }

            if (Input.GetMouseButton(0)) {
                MoveToCursor();
            }
            if (Input.GetKeyDown("left shift")) {
                mover.ToggleSprint();
            }
        }
        
        private void MoveToCursor() {
            var hasHit = Physics.Raycast(GetRay(), out var hit);
            if (hasHit) {
                mover.MoveTo(hit.point);
            }
        }

        private Ray GetRay() {
            return mainCamera.ScreenPointToRay(Input.mousePosition);
        }
    }
}