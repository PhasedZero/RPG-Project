using RPG.Core;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement {
    public class Mover : MonoBehaviour, IAction, ISavable {
        [SerializeField] private float maxSpeed = 5.66f;

        private NavMeshAgent navMeshAgent;
        private Animator animator;

        private static readonly int ForwardSpeed = Animator.StringToHash("forwardSpeed");
        private ActionScheduler actionScheduler;
        private Health health;

        private void Awake() {
            health = GetComponent<Health>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update() {
            navMeshAgent.enabled = !health.IsDead;
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction) {
            actionScheduler.StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction) {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(destination);
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
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

        public void UpdateMovementSpeed(float speedFraction) {
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
        }

        public object CaptureState() {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state) {
            var serializableVector3 = (SerializableVector3) state;
            actionScheduler.CancelCurrentAction();
            navMeshAgent.enabled = false;
            transform.position = serializableVector3.ToVector3();
            navMeshAgent.enabled = true;
        }
    }
}
