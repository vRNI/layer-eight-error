
using Boo.Lang.Runtime;
using JetBrains.Annotations;
using UnityEngine;

public static class ValidityManager
{
    /// <summary>
    /// Searches the manager node for a certain manager.
    /// </summary>
    /// <typeparam name="TManager">
    /// The type of manager to search for.
    /// </typeparam>
    /// <returns>
    /// The manager to search for.
    /// </returns>
    /// <exception cref="RuntimeException">
    /// More than one manager node exists in the scene, or the manager to search for does not exist.
    /// </exception>
    [ NotNull ]
    public static TManager FindManager< TManager > ()
        where TManager : MonoBehaviour
    {
        var managerNodes = GameObject.FindGameObjectsWithTag( TagName.ManagerNode );

        if ( managerNodes.Length != 1 ) { throw new RuntimeException( "More than one game obejct has the '" + TagName.ManagerNode + "' tag." ); }

        var manager      = managerNodes[ 0 ].GetComponent< TManager >();

        if ( manager == null ) { throw new RuntimeException( "Manager of type '" + typeof( TManager ).Name + "' was not added to manager node." ); }

        return manager;
    }

    /// <summary>
    /// Finds the player in the scene.
    /// </summary>
    /// <returns>
    /// The player game object.
    /// </returns>
    [ NotNull ]
    public static GameObject FindPlayer()
    {
        var playerObjects = GameObject.FindGameObjectsWithTag( TagName.Player );

        if ( playerObjects.Length != 1 ) { throw new RuntimeException( "Player can not be located ( there are '" + playerObjects.Length + "' players in the scene." ); }

        return playerObjects[ 0 ];
    }

    /// <summary>
    /// Gets the ortho-perspective switcher.
    /// </summary>
    /// <returns>
    /// The ortho-perspective switcher.
    /// </returns>
    [ NotNull ]
    public static OrthoPerspectiveSwitcher FindOrthoPerspectiveSwitcher()
    {
        var camera = Camera.main;
        var switcher = camera.GetComponent< OrthoPerspectiveSwitcher >();

        if ( switcher == null ) { throw new RuntimeException( "The active main camera has no ortho-perspective switcher." ); }

        return switcher;
    }

    /// <summary>
    /// Terminate the application because of an unexpected error.
    /// </summary>
    public static void TerminateUnexpectedly( string a_errorMessage = null )
    {
        Debug.Log( "An unexpected error occurred." );
        if ( string.IsNullOrEmpty( a_errorMessage ) == false ) { Debug.Log( a_errorMessage ); }

        throw new InitializationErrorException( a_errorMessage );
    }
}
