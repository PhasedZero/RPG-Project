using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement {
    public class Mover : MonoBehaviour, IAction {
        [SerializeField] private float runSpeed = 5.66f;
        [SerializeField] private float walkSpeed = 3f;

        private NavMeshAgent navMeshAgent;
        private Animator animator;

        private static readonly int ForwardSpeed = Animator.StringToHash("forwardSpeed");
        private ActionScheduler actionScheduler;
        private Health health;
        private bool isSprinting;

        public bool IsSprinting {
            get => isSprinting;
            set {
                isSprinting = value;
                UpdateMoveSpeed();
            } 
        }

        private void Start() {
            health = GetComponent<Health>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            UpdateMoveSpeed();
        }

        private void Update() {
            navMeshAgent.enabled = !health.IsDead;
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination) {
            actionScheduler.StartAction(this);
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination) {
            navMeshAgent.SetDestination(destination);
            navMeshAgent.isStopped = false;
        }
        
        public void Cancel() {
            navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator() {
            var velocity = navMeshAgent.velocity;
            var localVelocity = transform.InverseTransformDirection(velocity);
            var speed = localVelocity.z;
            animator.SetFloat(ForwardSpeed, speed);
        }

        public void ToggleSprint() {
            IsSprinting = !IsSprinting;
            UpdateMoveSpeed();
        }
        
        private void UpdateMoveSpeed() {
            navMeshAgent.speed = IsSprinting ? runSpeed : walkSpeed;
        }
    }
}
