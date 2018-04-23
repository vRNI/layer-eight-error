using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerWalk : MonoBehaviour {

    private NavMeshAgent navMeshAgent;
    private RaycastHit hitInformation;

	// Use this for initialization
	void Start () {
        navMeshAgent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInformation))
            {
                navMeshAgent.SetDestination(hitInformation.point);
            }
        }
	}
}
