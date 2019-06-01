using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour {
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float walkSpeed = 5f;
    private NavMeshAgent navMeshAgent;
    
    private bool isSprinting;
    private Camera mainCamera;

    // Start is called before the first frame update
    private void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    private void Update() {
        ProcessControls();
    }

    private void ProcessControls() {
        if (Input.GetMouseButtonDown(0)) {
            SetMoveSpeed(Input.GetKey("left ctrl"));
            MoveToCursor();
        }
        
        if (Input.GetKeyDown("left shift")) {
            ToggleSprint();
        }
    }

    private void SetMoveSpeed(bool sprint) {
        navMeshAgent.speed = sprint ? runSpeed : walkSpeed;
        this.isSprinting = sprint;
    }

    private void MoveToCursor() {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        var hasHit = Physics.Raycast(ray, out var hit);
        if (hasHit) {
            navMeshAgent.SetDestination(hit.point);
        }
    }

    private void ToggleSprint() {
        isSprinting = !isSprinting;
        Sprint(isSprinting);
    }

    private void Sprint(bool sprint) {
        navMeshAgent.speed = sprint ? runSpeed : walkSpeed;
    }
}