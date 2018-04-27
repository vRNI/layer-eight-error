using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailGameState
    : GameState
{
    public override void Enter()
    {
        base.Enter();

        // create ui elements
        var prefabManager   = Finder.GetPrefabs();
        var heading         = prefabManager.InstantiateFailStateHeading();
        var playerPosition  = Finder.GetPlayer().GetComponent< Transform >().position;
        var headingPosition = playerPosition + new Vector3( 0.0f, 5.0f, 0.0f );
        heading.GetComponent< Transform >().position = headingPosition;
        var title           = prefabManager.InstantiateFailStateTitle();
        var titlePosition   = headingPosition + new Vector3( 0.0f, 0.0f, 7.50f );
        title.GetComponent< Transform >().position = titlePosition;
        var restart         = prefabManager.InstantiateFailStateRestart();
        var restartPosition = headingPosition + new Vector3( -7.50f, 0.0f, -7.50f );
        restart.GetComponent< Transform >().position = restartPosition;
        var exit            = prefabManager.InstantiateFailStateExit();
        var exitPosition    = headingPosition + new Vector3( 7.50f, 0.0f, -7.50f );
        exit.GetComponent< Transform >().position = exitPosition;
        {
            var us              = prefabManager.InstantiateFailStateUs();
            var usPosition      = headingPosition + new Vector3( -10.0f, 0.0f, 0.0f );
            us.GetComponent< Transform >().position = usPosition;
        }
        {
            var us              = prefabManager.InstantiateFailStateUs();
            var usPosition      = headingPosition + new Vector3( 10.0f, 0.0f, 0.0f );
            us.GetComponent< Transform >().position = usPosition;
        }
        // start camera transitions
        var cameraOrbitScript   = Finder.GetCameraOrbitScript();
        cameraOrbitScript.SetMinCameraDistance( 25.0f );
        cameraOrbitScript.LocalRotation      = new Vector3( 0.0f, 90.0f, 0.0f );

        // set all entites non-hostile
        foreach ( var leaderEntity in Object.FindObjectsOfType< LeaderEntity >() )
        {
            if ( leaderEntity == Finder.GetPlayer() ) { continue; }
            leaderEntity.SetUnderlingsHostility( false );
        }
    }
}
