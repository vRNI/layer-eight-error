
using System.Linq;
using Boo.Lang.Runtime;
using UnityEngine;

public class FormationGameState
    : GameState
{
    private Vector3 m_originalOrbitRotation;
    private GameObject[] m_gridSlotObjects;
    private GameObject[] m_proxyObjects;
    private GameObject m_draggingObject;

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
        // EntityType.None is preserved for the empty slot prefab
        var emptySlotPrefab        = prefabsManager.GetEntityProxies().Where( a_x => a_x.EntityType == EntityType.None ).Select( a_x => a_x.ProxyPrefab ).SingleOrDefault();

        m_gridSlotObjects = new GameObject[ formationConfiguration.GetSlotCount() ];
        
        int i = -1;
        foreach ( var slotPosition in formationConfiguration.EnumerateSlotPosition() )
        {
            m_gridSlotObjects[ ++i ] = Object.Instantiate( emptySlotPrefab );
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

        if ( m_draggingObject == null )
        {
            // remember game object if player clicked on a proxy
            if ( Input.GetMouseButtonDown( MouseButtonIndex.Left ) == true )
            {
                RaycastHit hit;
                if ( MathUtil.RaycastFromMousePointer( out hit ) == true )
                {
                    var hitObject = hit.collider.gameObject;
                    var dragDropTarget = hitObject.GetComponent< FormationEditorDragDropTarget >();
                    // ignore object if it is no drag drop target or an empty slot
                    if ( dragDropTarget == null || dragDropTarget.GetEntityType() == EntityType.None ) { return; }
                    // ignore in future ray casts until object is dropped again
                    hitObject.layer = LayerMask.NameToLayer( LayerName.IgnoreRaycast );
                    m_draggingObject = hitObject;
                }
            }
        }
        else
        {
            // check if player releases dragged object
            if ( Input.GetMouseButtonUp( MouseButtonIndex.Left ) == true )
            {
                RaycastHit hit;
                if ( MathUtil.RaycastFromMousePointer( out hit ) == false ) { return; }

                var dragDropTarget = hit.collider.gameObject.GetComponent< FormationEditorDragDropTarget >();

                // set it to new grid position
                if ( dragDropTarget != null && dragDropTarget.GetEntityType() == EntityType.None )
                {
                    var dragDropSource       = m_draggingObject.GetComponent< FormationEditorDragDropTarget >();
                    var slotPositionSource   = dragDropSource.SlotPosition;
                    var slotPositionTarget   = dragDropTarget.SlotPosition;
                    var gridSlotObjectSource = m_gridSlotObjects.Select( a_x => a_x.GetComponent< FormationEditorDragDropTarget >() ).Single( a_x => a_x.SlotPosition.X == slotPositionSource.X && a_x.SlotPosition.Z == slotPositionSource.Z );
                    var gridSlotObjectTarget = m_gridSlotObjects.Select( a_x => a_x.GetComponent< FormationEditorDragDropTarget >() ).Single( a_x => a_x.SlotPosition.X == slotPositionTarget.X && a_x.SlotPosition.Z == slotPositionTarget.Z );

                    // set new grid position for drop source
                    dragDropSource.GetComponent< FormationEditorDragDropTarget >().SlotPosition = slotPositionTarget;
                    // snap to grid position
                    dragDropSource.GetComponent< Transform >().position = dragDropTarget.GetComponent< Transform >().position;
                    // deactivate grid slot target
                    gridSlotObjectTarget.gameObject.SetActive( false );
                    // activate grid slot source
                    gridSlotObjectSource.gameObject.SetActive( true );
                    // write new slot position to non-proxy entity
                    var player                 = Finder.GetPlayer();
                    var formationConfiguration = player.GetComponent< FormationConfiguration >();
                    var entities               = formationConfiguration.GetUnderlingEntities();
                    var entity                 = entities.Single( a_x => a_x.GetFormationSlot().X == slotPositionSource.X && a_x.GetFormationSlot().Z == slotPositionSource.Z );
                    entity.SetFormationSlot( slotPositionTarget );
                }
                else // reset to original position
                {
                    var dragDropSource   = m_draggingObject.GetComponent< FormationEditorDragDropTarget >();
                    var slotPosition     = dragDropSource.SlotPosition;
                    var gridSlotObject   = m_gridSlotObjects.Select( a_x => a_x.GetComponent< FormationEditorDragDropTarget >() ).SingleOrDefault( a_x => a_x.SlotPosition.X == slotPosition.X && a_x.SlotPosition.Z == slotPosition.Z );
                    if ( gridSlotObject == null ) { throw new RuntimeException( "No grid slot object exists to restore original position of entity proxy." ); }
                    var originalPosition = gridSlotObject.gameObject.GetComponent< Transform >().position;
                    m_draggingObject.gameObject.GetComponent< Transform >().position = originalPosition;
                }

                // reset layer to default
                m_draggingObject.layer = LayerMask.NameToLayer( LayerName.Default );
                // then clear dragging object
                m_draggingObject = null;
            }
            else // follow mouse cursor
            {
                RaycastHit hit;
                if ( MathUtil.RaycastFromMousePointer( out hit ) == true )
                {
                    var hitPosition     = hit.point;
                    var currentPosition = m_draggingObject.GetComponent< Transform >().position;
                    var newPosition     = new Vector3( hitPosition.x, currentPosition.y, hitPosition.z );

                    m_draggingObject.GetComponent< Transform >().position = newPosition;
                }
            }
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
