using System;
using UnityEngine;

namespace RPG.Combat {
    public class Projectile : MonoBehaviour {
        [SerializeField] private float speed = 10f;
        [SerializeField] private Transform currentTarget = null;
        private Collider targetCollider;

        private void Update() {
            if (!currentTarget) return;
            transform.LookAt(GetVector());
            transform.Translate(speed * Time.deltaTime * Vector3.forward);
        }

        private Vector3 GetVector() {
            if (!targetCollider) {
                targetCollider = currentTarget.GetComponent<Collider>();
            }

            if (!targetCollider) {
                return currentTarget.position;
            }
            return targetCollider.bounds.center;
        }

        public void SetTarget(Transform target) {
            currentTarget = target;
        }

        private void OnTriggerEnter(Collider other) {
            Destroy(gameObject);
        }
    }
}