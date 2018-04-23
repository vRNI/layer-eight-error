using UnityEngine;

public class IdleState
    : EntityState
{

    public IdleState()
    {
    }

    public override void Update()
    {
        Debug.Log("Entity.IdleState.Update()");
    }
}
