using UnityEngine;

namespace RPG.Control {
    public class PatrolPath : MonoBehaviour {
        private const float waypointGizmoRadius = .3f;

        private void OnDrawGizmos() {
            Gizmos.color = Color.yellow;

            for (var i = 0; i < transform.childCount; i++) {
                int j = GetNextIndex(i);

                Gizmos.DrawSphere(GetPosition(i), waypointGizmoRadius);
                Gizmos.DrawLine(GetPosition(i), GetPosition(j));
            }
        }
        
        private int GetNextIndex(int index) {
            return (index + 1) % transform.childCount;
        }

        private Vector3 GetPosition(int i) {
            return transform.GetChild(i).position;
        }
    }
}
