﻿using UnityEngine;

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
    /// Gets the one and only formation configuration.
    /// </summary>
    public static FormationConfiguration GetFormationConfiguration()
    {
        return Object.FindObjectOfType< FormationConfiguration >();
    }
    
    /// <summary>
    /// Gets the one and only ortho perspective changer.
    /// </summary>
    public static OrthoPerspectiveSwitcher GetOrthoPerspectiveSwitcher()
    {
        return Camera.main.GetComponent< OrthoPerspectiveSwitcher >();
    }

    /// <summary>
    /// Gets the player position.
    /// </summary>
    public static Vector3 GetPlayerCurrentPosition()
    {
        return GameObject.FindGameObjectWithTag( TagName.Player ).GetComponent< Transform >().position;
    }

    /// <summary>
    /// Gets the players desired position.
    /// </summary>
    public static Vector3 GetPlayerDesiredPosition()
    {
        return GameObject.FindGameObjectWithTag( TagName.Player ).GetComponent< PlayerMouseController >().GetDesiredPosition();
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
