using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OverlordAnimationInfo : MonoBehaviour {
    Animator animator;

    [SerializeField] private float speed;
    [SerializeField] private bool hostile;
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    //public void TriggerAttack()
    //{
    //    var rand = (int)Random.Range(0, 10);
    //    animator.SetInteger("Random", rand);
    //    animator.SetTrigger("Attack");
    //}

    public void TriggerDead()
    {
        var rand = (int)Random.Range(1, 10);
        animator.SetInteger("Random", rand);
        animator.SetTrigger("Dead");

    }

    // Update is called once per frame
    void Update () {
        var velocity = gameObject.GetComponent<NavMeshAgent>().velocity;
        speed = Vector3.SqrMagnitude(velocity);
        animator.SetFloat("Velocity", speed);
    }
}
