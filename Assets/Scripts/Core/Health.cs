using UnityEngine;

namespace RPG.Core {
    public class Health : MonoBehaviour {
        [SerializeField] private float healthPoints = 100f;
        private Animator animator;

        public bool IsDead { get; private set; } = false;

        private void Start() {
            animator = GetComponent<Animator>();
        }

        public void TakeDamage(float damage) {
            healthPoints = Mathf.Max(healthPoints - damage, 0f);
            print(healthPoints);
            if (healthPoints <= 0) {
                DeathSequence();
            }
        }

        private void DeathSequence() {
            if (IsDead) return;
            animator.SetBool("death",true);
            IsDead = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

    }
}
