using UnityEngine;

namespace RPG.Control {
    public class PatrolPath : MonoBehaviour {
        private const float WaypointGizmoRadius = .3f;

        private void OnDrawGizmos() {
            Gizmos.color = Color.yellow;

            for (var i = 0; i < transform.childCount; i++) {

                Gizmos.DrawSphere(GetPosition(i), WaypointGizmoRadius);
                Gizmos.DrawLine(GetPosition(i), GetNextPosition(i));
            }
        }
        
        public Vector3 GetNextPosition(int index) {
            return GetPosition((index + 1) % transform.childCount);
        }

        public Vector3 GetPosition(int i) {
            return transform.GetChild(i).position;
        }
    }
}
