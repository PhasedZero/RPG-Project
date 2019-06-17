using RPG.Core;
using UnityEngine;

namespace RPG.Combat {
    public class Projectile : MonoBehaviour {
        [SerializeField] private float speed = 10f;
        [SerializeField] private float projectileDamage = 2f;
        [SerializeField] private bool isHoming;
        
        private Health currentTarget = null;
        private Collider targetCollider;
        private float weaponDamage = 0f;

        private void Update() {
            if (!currentTarget) return;
            if (isHoming) {
                transform.LookAt(GetVector());
            }
            
            transform.Translate(speed * Time.deltaTime * Vector3.forward);
        }

        private Vector3 GetVector() {
            if (!targetCollider) {
                return currentTarget.transform.position;
            }
            return targetCollider.bounds.center;
        }

        public void SetTargetAndDamage(Health target, float damage) {
            currentTarget = target;
            weaponDamage = damage;
            targetCollider = currentTarget.GetComponent<Collider>();
            transform.LookAt(GetVector());
        }

        private void OnTriggerEnter(Collider other) {
            var otherHealth = other.GetComponent<Health>();
            if (otherHealth) {
                currentTarget.TakeDamage(projectileDamage + weaponDamage);
            }
            Destroy(gameObject);
        }
    }
}