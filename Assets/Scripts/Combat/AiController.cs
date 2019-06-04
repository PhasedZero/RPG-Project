using UnityEngine;

namespace RPG.Combat {
    public class AiController : MonoBehaviour {
        [SerializeField] private float chaseDistance = 5f;

        private GameObject player;
        private Fighter fighter;

        private bool isChasing = false;

        private void Start() {
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
        }

        private void Update() {
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
    }
}
