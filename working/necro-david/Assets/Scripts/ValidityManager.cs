
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
}
