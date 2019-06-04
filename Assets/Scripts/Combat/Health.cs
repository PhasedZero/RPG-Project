using UnityEngine;

namespace RPG.Combat {
    public class Health : MonoBehaviour {
        [SerializeField] private float health = 100f;
        private Animator animator;

        private void Start() {
            animator = GetComponent<Animator>();
        }
        
        public void TakeDamage(float damage) {
            health = Mathf.Max(health - damage, 0f);
            print(health);
            if (health <= 0) {
                DeathSequence();
            }
        }
        
        private void DeathSequence() {
            animator.SetBool("death",true);
        }
    }
}
