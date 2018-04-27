using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationInfo : MonoBehaviour {

    Animator animator;

    [SerializeField] private float speed;
    [SerializeField] private bool hostile;
    private GameObject undead;
    private GameObject human;
    [SerializeField] private Avatar undeadAvatar;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        human = gameObject.transform.Find("WK").gameObject;
        undead = gameObject.transform.Find("UD").gameObject;
        //if (GetComponent<BaseEntity>().GetEntityType() == EntityType.Fighter)
        //{
        //    human = gameObject.transform.Find("WK_Fighter").gameObject;
        //    undead = gameObject.transform.Find("UD_Fighter").gameObject;
        //}
        //if (GetComponent<BaseEntity>().GetEntityType() == EntityType.Mage)
        //{
        //    human = gameObject.transform.Find("WK_Mage").gameObject;
        //    undead = gameObject.transform.Find("UD_Mage").gameObject;
        //}
        //if (GetComponent<BaseEntity>().GetEntityType() == EntityType.Ranger)
        //{
        //    human = gameObject.transform.Find("WK_Ranger").gameObject;
        //    undead = gameObject.transform.Find("UD_Ranger").gameObject;
        //}

    }

    public void SwitchModel()
    {
        human.SetActive(false);
        undead.SetActive(true);
        GetComponent<Animator>().avatar = undeadAvatar;
    }

    public void Resurect()
    {
        animator.SetTrigger("Resurect");
    }

    public void TriggerAttack()
    {
        var rand = (int) Random.Range(0, 10);
        animator.SetInteger("Random", rand);
        animator.SetTrigger("Attack");
    }

    public void TriggerDead()
    {
        var rand = (int)Random.Range(1, 10);
        animator.SetInteger("Random", rand);
        animator.SetTrigger("Dead");
        
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
