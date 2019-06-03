using RPG.Combat;
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
        private Fighter fighter;

        private void Start() {
            fighter = GetComponent<Fighter>();
            animator = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update() {
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination) {
            fighter.Cancel();
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination) {
            navMeshAgent.SetDestination(destination);
            navMeshAgent.isStopped = false;
        }
        
        public void Stop() {
            navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator() {
            var velocity = navMeshAgent.velocity;
            var localVelocity = transform.InverseTransformDirection(velocity);
            var speed = localVelocity.z;
            animator.SetFloat(ForwardSpeed, speed);
        }

        public void ToggleSprint() {
            isSprinting = !isSprinting;
            navMeshAgent.speed = isSprinting ? runSpeed : walkSpeed;
        }
    }
}
