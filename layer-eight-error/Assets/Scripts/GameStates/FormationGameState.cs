
using System.Linq;
using Boo.Lang.Runtime;
using UnityEngine;

public class FormationGameState
    : GameState
{
    private Vector3 m_originalOrbitRotation;
    private GameObject[] m_gridSlotObjects;
    private GameObject[] m_proxyObjects;

    public override void Enter()
    {
        base.Enter();

        var cameraOrbitScript   = Finder.GetCameraOrbitScript();
        m_originalOrbitRotation = cameraOrbitScript.LocalRotation;
        
        var player                 = Finder.GetPlayer();
        var playerTransform        = player.GetComponent< Transform >();
        var playerPosition         = playerTransform.position;
        var playerEulerAngles      = playerTransform.eulerAngles;

        cameraOrbitScript.LocalRotation      = new Vector3( playerEulerAngles.y, 90.0f, m_originalOrbitRotation.z );
        cameraOrbitScript.AreControlsEnabled = false;
        Finder.GetOrthoPerspectiveSwitcher().SwitchToOrtho();

        var playerRotationY        = playerEulerAngles.y;
        var formationConfiguration = player.GetComponent< FormationConfiguration >();
        var entities               = formationConfiguration.GetUnderlingEntities();
        var prefabsManager         = Finder.GetPrefabs();

        m_gridSlotObjects = new GameObject[ formationConfiguration.GetSlotCount() ];
        
        int i = -1;
        foreach ( var slotPosition in formationConfiguration.EnumerateSlotPosition() )
        {
            m_gridSlotObjects[ ++i ] = Object.Instantiate( prefabsManager.GetEmptyFormationSlot() );
            m_gridSlotObjects[ i ].GetComponent< FormationEditorDragDropTarget >().SlotPosition = slotPosition;

            var slotOffset             = formationConfiguration.GetSlotOffset( slotPosition );
            var slotOffsetRotated      = Quaternion.AngleAxis( playerRotationY, Vector3.up ) * slotOffset;
            var targetPosition         = playerPosition + slotOffsetRotated + new Vector3( 0.0f, 2.0f, 0.0f ); // move stuff a bit upwards

            // player is in grid center
            if ( slotPosition.X == 0 && slotPosition.Z == 0 )
            {
                m_gridSlotObjects[ i ].SetActive( false );
            }

            m_gridSlotObjects[ i ].GetComponent< Transform >().position   = targetPosition;
            m_gridSlotObjects[ i ].GetComponent< Transform >().localScale = new Vector3( 0.3f, 0.3f, 0.3f );
        }

        m_proxyObjects    = new GameObject[ entities.Length ];

        // disable already used slots
        int j = -1;
        foreach ( var entity in entities )
        {
            var slotPosition      = entity.GetFormationSlot();
            var gridSlotObject    = m_gridSlotObjects.SingleOrDefault( a_x => AreEqual( a_x.GetComponent< FormationEditorDragDropTarget >().SlotPosition, slotPosition ) );
            if ( gridSlotObject == null ) { continue; }
            gridSlotObject.SetActive( false );
            // place entity proxy here
            var proxyPrefab    = prefabsManager.GetEntityProxies().Where( a_x => a_x.EntityType == entity.GetEntityType() ).Select( a_x => a_x.ProxyPrefab ).SingleOrDefault();
            if ( proxyPrefab == null ) { throw new RuntimeException( "No proxy prefab is defined for entity type '" + entity.GetEntityType() + "'." ); }
            m_proxyObjects[ ++j ] = Object.Instantiate( proxyPrefab );
            m_proxyObjects[ j ].GetComponent< FormationEditorDragDropTarget >().SlotPosition = slotPosition;
            m_proxyObjects[ j ].GetComponent< Transform >().eulerAngles = new Vector3( 0.0f, playerRotationY, 0.0f );
            m_proxyObjects[ j ].GetComponent< Transform >().position    = gridSlotObject.GetComponent< Transform >().position;
        }

        // deactivate all friendly entities
        foreach ( var entity in entities )
        {
            entity.gameObject.SetActive( false );
        }
    }

    private bool AreEqual( Position2 a_one, Position2 a_other )
    {
        return a_one.X == a_other.X && a_one.Z == a_other.Z;
    }

    public override void Update()
    {
        base.Update();

        // check if the player activated the idle state
        if ( Input.GetButtonDown( AxisName.ToggleFormationMode ) == true )
        {
            TriggerTransition< IdleGameState >();
            return;
        }
    }

    public override void Exit()
    {
        var player                 = Finder.GetPlayer();
        var formationConfiguration = player.GetComponent< FormationConfiguration >();
        var entities               = formationConfiguration.GetUnderlingEntities();

        // re-activate all friendly entities
        foreach ( var entity in entities )
        {
            entity.gameObject.SetActive( true );
        }

        // delete proxies
        foreach (var proxyObject in m_proxyObjects)
        {
            Object.Destroy( proxyObject );
        }

        // delete grid slot objects
        foreach ( var slotObject in m_gridSlotObjects )
        {
            Object.Destroy( slotObject );
        }

        var cameraOrbitScript = Finder.GetCameraOrbitScript();

        Finder.GetOrthoPerspectiveSwitcher().SwitchToPerspective();
        cameraOrbitScript.AreControlsEnabled = true;
        cameraOrbitScript.LocalRotation      = m_originalOrbitRotation;

        base.Exit();
    }
}
