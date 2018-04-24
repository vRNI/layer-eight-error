
using UnityEngine;
using UnityEngine.AI;

public class WalkPlayerState
    : PlayerState
{
    public override void Enter()
    {
        base.Enter();

        Debug.Log( "Player starts walking." );
    }

    public override void Update()
    {
        base.Update();
        
        if ( Finder.GetGameStateManager().IsCurrentState< FormationGameState >() )
        {
            // Stop walking
            Seek( Finder.GetPlayer().GetComponent< Transform >().position );
            Finder.GetPlayerStateManager().SetCurrentState< IdlePlayerState >();
            return;
        }

        var formationConfiguration = m_player.GetComponent< FormationConfiguration >();

        var currentPosition = Finder.GetCurrentPosition( m_player );
        var desiredPosition = Finder.GetDesiredPosition( m_player );

        // if desired player position is far enough away from current position start walking
        if ( Vector3.Distance( currentPosition, desiredPosition ) > formationConfiguration.GetFollowThreshold() )
        {
            Seek( desiredPosition );
        }
        else // stop walking
        {
            Finder.GetPlayerStateManager().SetCurrentState< IdlePlayerState >();
        }
    }

    private void Seek( Vector3 a_destination )
    {
        var player       = Finder.GetPlayer();
        var navMeshAgent = player.GetComponent< NavMeshAgent >();
        navMeshAgent.SetDestination( a_destination );
    }
}
