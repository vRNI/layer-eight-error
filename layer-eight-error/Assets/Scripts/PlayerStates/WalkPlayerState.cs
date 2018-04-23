
using UnityEngine;
using UnityEngine.AI;

public class WalkPlayerState
    : PlayerState
{
    public override void Update()
    {
        base.Update();

        var formationConfiguration = Finder.GetFormationConfiguration();

        var currentPosition = Finder.GetPlayerCurrentPosition();
        var desiredPosition = Finder.GetPlayerDesiredPosition();

        // if desired player position is far enough away from current position start walking
        if ( Vector3.Distance( currentPosition, desiredPosition ) > formationConfiguration.GetFollowThreshold() )
        {
            var player       = Finder.GetPlayer();
            var navMeshAgent = player.GetComponent< NavMeshAgent >();
             navMeshAgent.SetDestination( desiredPosition );
        }
        else // stop walking
        {
            Finder.GetPlayerStateManager().SetCurrentState< IdlePlayerState >();
        }
    }
}
