using UnityEngine;

namespace RPG.Control {
    public class PatrolPath : MonoBehaviour {
        private const float waypointGizmoRadius = .3f;
        
        private void OnDrawGizmos() {
            Gizmos.color = Color.yellow;
            
            for (var i = 0; i < transform.childCount; i++) {
                Gizmos.DrawSphere(transform.GetChild(i).position, waypointGizmoRadius);
                if (i == 0) {
                    Gizmos.DrawLine(transform.GetChild(transform.childCount - 1).position, transform.GetChild(i).position);
                }
                else {
                    Gizmos.DrawLine(transform.GetChild(i - 1).position, transform.GetChild(i).position);
                }
            }
        }
    }
}
