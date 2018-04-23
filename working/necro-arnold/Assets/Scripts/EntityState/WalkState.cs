using UnityEngine;

public class WalkState 
    : EntityState
{

	public WalkState()
    {
    }
	
	public override void Update()
    {
        Debug.Log("Entity.WalkState.Update()");
	}
}
