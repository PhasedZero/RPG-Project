using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement {
    public class Mover : MonoBehaviour {
        [SerializeField] private float runSpeed = 5.66f;
        [SerializeField] private float walkSpeed = 3f;
    
        private NavMeshAgent navMeshAgent;
        private Animator animator;

        private bool isSprinting;
        private static readonly int ForwardSpeed = Animator.StringToHash("forwardSpeed");

        private void Start() {
            animator = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update() {
            UpdateAnimator();
        }

        private void UpdateAnimator() {
            var velocity = navMeshAgent.velocity;
            var localVelocity = transform.InverseTransformDirection(velocity);
            var speed = localVelocity.z;
            animator.SetFloat(ForwardSpeed, speed);
        }
        
        public void MoveTo(Vector3 destination) {
            navMeshAgent.SetDestination(destination);
        }

        public void ToggleSprint() {
            isSprinting = !isSprinting;
            navMeshAgent.speed = isSprinting ? runSpeed : walkSpeed;
        }
    }
}