using UnityEngine;

/// <summary>
/// Encapsulates searching within unity objects for specific stuff.
/// </summary>
public static class Finder
{
    /// <summary>
    /// Gets the one and only game state manager.
    /// </summary>
    public static GameStateManager GetGameStateManager()
    {
        return Object.FindObjectOfType< GameStateManager >();
    }
    
    /// <summary>
    /// Gets the one and only prefabs manager.
    /// </summary>
    public static PrefabsManager GetPrefabs()
    {
        return Object.FindObjectOfType< PrefabsManager >();
    }

    /// <summary>
    /// Gets the one and only ortho perspective changer.
    /// </summary>
    public static OrthoPerspectiveSwitcher GetOrthoPerspectiveSwitcher()
    {
        return Camera.main.GetComponent< OrthoPerspectiveSwitcher >();
    }

    /// <summary>
    /// Gets the one and only camera orbit script.
    /// </summary>
    public static CameraOrbitScript GetCameraOrbitScript()
    {
        return Camera.main.GetComponent< CameraOrbitScript >();
    }

    /// <summary>
    /// Gets the formation leaders current position.
    /// </summary>
    /// <param name="a_formationLeader">
    /// The formation leader to follow.
    /// </param>
    public static Vector3 GetCurrentPosition( GameObject a_formationLeader )
    {
        if ( a_formationLeader == null ) { return Vector3.zero; }

        return a_formationLeader.GetComponent< Transform >().position;
    }
    
    /// <summary>
    /// Gets the formation leaders desired position.
    /// </summary>
    /// <param name="a_formationLeader">
    /// The formation leader to follow.
    /// </param>
    public static Vector3 GetDesiredPosition( GameObject a_formationLeader )
    {
        if ( a_formationLeader == null ) { return Vector3.zero; }
        if ( a_formationLeader.CompareTag( TagName.Player ) == true ) { return a_formationLeader.GetComponent< PlayerMouseController >().GetDesiredPosition(); }

        return a_formationLeader.GetComponent< Transform >().position;
    }

    /// <summary>
    /// Gets the player height as 3D vector ( only y component is set, x and z are 0.0f ).
    /// </summary>
    public static Vector3 GetPlayerHeight()
    {
        var player  = GetPlayer();
        var capsule = player.GetComponent< CapsuleCollider >();

        return new Vector3( 0.0f, capsule.height, 0.0f );
    }

    /// <summary>
    /// Gets the one and only player game object.
    /// </summary>
    public static GameObject GetPlayer()
    {
        return GameObject.FindGameObjectWithTag( TagName.Player );
    }

    /// <summary>
    /// Gets the one and only player state manager.
    /// </summary>
    public static PlayerStateManager GetPlayerStateManager()
    {
        return GameObject.FindGameObjectWithTag( TagName.Player ).GetComponent< PlayerStateManager >();
    }
}
