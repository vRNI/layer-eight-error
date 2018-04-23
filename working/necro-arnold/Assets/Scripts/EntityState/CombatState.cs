using UnityEngine;

public class CombatState
    : EntityState
{
    public CombatState()
    {
    }

    public override void Update()
    {
        Debug.Log("Entity.CombatState.Update()");
    }
}