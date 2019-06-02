using Movement;
using UnityEngine;

namespace Control {
    public class PlayerController : MonoBehaviour {
        
        private Camera mainCamera;
        private Mover mover;

        // Start is called before the first frame update
        private void Start() {
            mover = GetComponent<Mover>();
            mainCamera = Camera.main;
        }

        // Update is called once per frame
        private void Update() {
            ProcessControls();
        }
        
        private void ProcessControls() {
            if (Input.GetMouseButton(0)) {
                MoveToCursor();
            }
            if (Input.GetKeyDown("left shift")) {
                mover.ToggleSprint();
            }
        }
        
        private void MoveToCursor() {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            var hasHit = Physics.Raycast(ray, out var hit);
            if (hasHit) {
                mover.MoveTo(hit.point);
            }
        }
    }
}