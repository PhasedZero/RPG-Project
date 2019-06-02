using UnityEngine;

namespace Core {
    public class FollowCamera : MonoBehaviour {
        [SerializeField] private Transform target;

        // Start is called before the first frame update
        void Start() {
        }

        // Update is called once per frame
        private void Update() {
            transform.position = target.position;
        }
    }
}