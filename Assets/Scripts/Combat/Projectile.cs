using RPG.Core;
using UnityEngine;

namespace RPG.Combat {
    public class Projectile : MonoBehaviour {
        [SerializeField] private float speed = 10f;
        [SerializeField] private float projectileDamage = 2f;
        [SerializeField] private bool isHoming;
        [SerializeField] private bool onlyHitTarget = false;
        [SerializeField] private GameObject hitEffect;
        [SerializeField] private float maxLifeTime = 10f;
        [SerializeField] private GameObject[] destroyOnHit;
        [SerializeField] private float lifeAfterImpact = 2f;
        
        private Health currentTarget;
        private Collider targetCollider;
        private float weaponDamage = 0f;
        private Collider sourceCollider;
        private bool isAi;
        private float maxRange = 20f;
        private Vector3 startLocation;

        private void Awake() {
            startLocation = transform.position;
        }

        private void Update() {
            CheckRange();
            if (!currentTarget) Destroy(gameObject);
            if (isHoming && !currentTarget.IsDead) {
                transform.LookAt(GetVector());
            }

            transform.Translate(speed * Time.deltaTime * Vector3.forward);
        }

        private Vector3 GetVector() {
            return !targetCollider ? currentTarget.transform.position : targetCollider.bounds.center;
        }

        public void SetTargetAndDamageAndSource(Health target, float damage, Collider source) {
            currentTarget = target;
            weaponDamage = damage;
            targetCollider = currentTarget.GetComponent<Collider>();
            transform.LookAt(GetVector());
            sourceCollider = source;
            isAi = !sourceCollider.CompareTag("Player");
            Destroy(gameObject,maxLifeTime);
        }

        private void CheckRange() {
            if (Vector3.Distance(startLocation, transform.position) >= maxRange) {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other) {
            if (other == sourceCollider) return;

            var otherHealth = other.GetComponent<Health>();

            if (otherHealth) {
                if (otherHealth.IsDead) return;
                if (isAi && !other.CompareTag("Player")) return;
                if (onlyHitTarget && other != targetCollider) return;

                currentTarget.TakeDamage(projectileDamage + weaponDamage);
            }

            if (hitEffect) {
                var fx = Instantiate(hitEffect, transform.position, transform.rotation);
                Destroy(fx,2f);
            }

            foreach (var toDestroy in destroyOnHit) {
                Destroy(toDestroy);
            }

            Destroy(gameObject, lifeAfterImpact);
        }
    }
}