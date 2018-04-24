
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

        var formationConfiguration = m_player.GetComponent< FormationConfiguration >();

        var currentPosition = Finder.GetCurrentPosition( m_player );
        var desiredPosition = Finder.GetDesiredPosition( m_player );

        // if desired player position is far enough away from current position start walking
        if ( Vector3.Distance( currentPosition, desiredPosition ) > formationConfiguration.GetFollowThreshold() )
        {
            Finder.GetPlayerStateManager().SetCurrentState< WalkPlayerState >();
        }
    }
}
