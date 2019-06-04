using UnityEngine;

namespace RPG.Combat {
    public class AiController : MonoBehaviour {
        [SerializeField] private float chaseDistance = 5f;
        
        private GameObject player;

        private void Start() {
            player = GameObject.FindWithTag("Player");
        }
        
        private void Update() {
            if (InRange()) {
                print(name + " In range");
            }
        }
        
        private bool InRange() {
            var range = Vector3.Distance(transform.position, player.transform.position);
            return range <= chaseDistance;
        }
    }
}
