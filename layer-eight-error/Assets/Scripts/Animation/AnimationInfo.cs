using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationInfo : MonoBehaviour {

    Animator animator;

    [SerializeField] private float speed;
    [SerializeField] private bool hostile;
    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
    }

    public void TriggerAttack()
    {
        animator.SetTrigger("Attack");
    }
	
	// Update is called once per frame
	void Update ()
	{
        var velocity = GetComponent<NavMeshAgent>().velocity;
	    speed = Vector3.SqrMagnitude(velocity);
	    hostile = GetComponent<UnderlingEntity>().IsHostile;
        animator.SetFloat("Velocity", speed);
	    animator.SetBool("Hostile", hostile);
    }
}
