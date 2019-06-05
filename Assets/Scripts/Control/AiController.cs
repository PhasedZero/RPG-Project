using System;
using RPG.Combat;
using UnityEngine;

namespace RPG.Core {
    public class AiController : MonoBehaviour {
        [SerializeField] private float chaseDistance = 5f;

        private GameObject player;
        private Fighter fighter;

        private bool isChasing = false;
        private Health health;

        private void Start() {
            health = GetComponent<Health>();
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
        }

        private void Update() {
            if (health.IsDead) return;
            if (!isChasing && InChaseRange() && fighter.CanAttack(player)) {
                fighter.Attack(player);
                isChasing = true;

            }
            else if (isChasing && !InChaseRange()) {
                fighter.Cancel();
                isChasing = false;
            }
        }

        private bool InChaseRange() {
            return Vector3.Distance(transform.position, player.transform.position) <= chaseDistance;
        }
        
        // Editor functions Called by Unity
        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
