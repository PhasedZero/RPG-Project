using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour {
    [SerializeField] private Transform target;

    private NavMeshAgent navMeshAgent;
    private bool isSprinting = false;

    // Start is called before the first frame update
    private void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    private void Update() {
        navMeshAgent.SetDestination(target.position);
        if (Input.GetKeyDown("left shift")) {
            ToggleSprint();
        }
    }

    private void ToggleSprint() {
        isSprinting = !isSprinting;
        Sprint(isSprinting);
    }

    private void Sprint(bool sprint) {
        navMeshAgent.speed = sprint ? 7f : 3.5f;
    }
}