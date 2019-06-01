using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour {
    [SerializeField] private Transform target;

    private NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    private void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    private void Update() {
        navMeshAgent.SetDestination(target.position);
    }
}