using UnityEngine;
using UnityEngine.AI;

namespace Movement {
    public class Mover : MonoBehaviour {
        [SerializeField] private float runSpeed = 10f;
        [SerializeField] private float walkSpeed = 5f;
    
        private NavMeshAgent navMeshAgent;
        private Camera mainCamera;
        private Animator animator;

        private bool isSprinting;

        // Start is called before the first frame update
        private void Start() {
            animator = GetComponentInChildren<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            mainCamera = Camera.main;
        }

        // Update is called once per frame
        private void Update() {
            ProcessControls();
            UpdateAnimator();
        }

        private void UpdateAnimator() {
            var velocity = navMeshAgent.velocity;
            var localVelocity = transform.InverseTransformDirection(velocity);
            var speed = localVelocity.z;
            animator.SetFloat("forwardSpeed", speed);
        }

        private void ProcessControls() {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) {
                MoveToCursor();
            }
        
            if (Input.GetKeyDown("left shift")) {
                ToggleSprint();
            }
        }

        private void MoveToCursor() {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            var hasHit = Physics.Raycast(ray, out var hit);
            if (hasHit) {
                navMeshAgent.SetDestination(hit.point);
            }
        }

        private void ToggleSprint() {
            isSprinting = !isSprinting;
            navMeshAgent.speed = isSprinting ? runSpeed : walkSpeed;
        }
    }
}