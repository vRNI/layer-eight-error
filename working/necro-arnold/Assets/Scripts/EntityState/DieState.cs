using UnityEngine;

public class DieState
    : EntityState
{

    public DieState()
    {
    }

    public override void Update()
    {
        Debug.Log("Entity.DieState.Update()");
    }
}
