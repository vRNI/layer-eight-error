using UnityEngine;

public class BattleState
    : GameState
{
    public BattleState()
    {
    }

    public override void Update()
    {
        Debug.Log( "BattleState.Update()" );
    }
}
