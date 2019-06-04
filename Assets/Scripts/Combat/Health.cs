using UnityEngine;

namespace RPG.Combat {
    public class Health : MonoBehaviour {
        [SerializeField] private float healthPoints = 100f;
        private Animator animator;

        private bool isDead = false;

        public bool IsDead {
            get => isDead;
            set => isDead = value;
        }

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
            if (isDead) return;
            animator.SetBool("death",true);
            isDead = true;
        }

    }
}
