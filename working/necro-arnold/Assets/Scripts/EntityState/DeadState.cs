using UnityEngine;

public class DeadState
    : EntityState
{

    public DeadState()
    {
    }

    public override void Update()
    {
        Debug.Log("Entity.DeadState.Update()");
    }
}
