
using UnityEngine;

public class IdlePlayerState
    : PlayerState
{
    public override void Enter()
    {
        base.Enter();

        Debug.Log( "Player starts idleing." );
    }

    public override void Update()
    {
        base.Update();

        var formationConfiguration = Finder.GetFormationConfiguration();

        var currentPosition = Finder.GetPlayerCurrentPosition();
        var desiredPosition = Finder.GetPlayerDesiredPosition();

        // if desired player position is far enough away from current position start walking
        if ( Vector3.Distance( currentPosition, desiredPosition ) > formationConfiguration.GetFollowThreshold() )
        {
            Finder.GetPlayerStateManager().SetCurrentState< WalkPlayerState >();
        }
    }
}
