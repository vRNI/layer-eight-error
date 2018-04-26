
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
        var playerEntities         = formationConfiguration.GetUnderlingUnits();
        var prefabsManager         = Finder.GetPrefabs();
        // EntityType.None is preserved for the empty slot prefab
        var emptySlotPrefab        = prefabsManager.GetEntityProxies().Where( a_x => a_x.EntityType == EntityType.None ).Select( a_x => a_x.ProxyPrefab ).SingleOrDefault();

        m_gridSlotObjects = new GameObject[ formationConfiguration.GetSlotCount() ];
        
        int i = -1;
        foreach ( var slotPosition in formationConfiguration.EnumerateSlotPosition() )
        {
            m_gridSlotObjects[ ++i ] = Object.Instantiate( emptySlotPrefab );
            GetGridSlotDragDropTarget( m_gridSlotObjects[ i ] ).SlotPosition = slotPosition;

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

        m_proxyObjects    = new GameObject[ playerEntities.Count ];

        // disable already used slots
        int j = -1;
        foreach ( var entity in playerEntities )
        {
            var slotPosition      = entity.GetFormationSlot();
            var gridSlotObject    = GetGridSlotObject( slotPosition );
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

        // deactivate all entities
        foreach ( var entity in Finder.GetEntityManager().GetUnderlings() )
        {
            // own entities are replaced by proxies
            if ( entity.GetLeader() == Finder.GetPlayer() )
            {
                entity.gameObject.SetActive( false );
            }
            else // only render enemies -> deactivate all behaviors
            {
                foreach ( var component in entity.GetComponents< Behaviour >() )
                {
                    component.enabled = false;
                }
            }
        }
        foreach (var entity in Finder.GetEntityManager().GetDeadUnderlings() )
        {
            entity.gameObject.SetActive( false );
        }
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
                var dragDropSource = m_draggingObject.GetComponent< FormationEditorDragDropTarget >();

                // set it to new grid position
                if ( dragDropTarget != null && dragDropTarget.GetEntityType() == EntityType.None )
                {
                    var slotPositionTarget     = dragDropTarget.SlotPosition;
                    var gridSlotObjectTarget   = GetGridSlotObject( slotPositionTarget );
                    var slotPositionSource     = dragDropSource.SlotPosition;
                    var gridSlotObjectSource   = GetGridSlotObject( slotPositionSource );

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
                    var entities               = formationConfiguration.GetUnderlingUnits();
                    var entity                 = entities.Single( a_x => a_x.GetFormationSlot().X == slotPositionSource.X && a_x.GetFormationSlot().Z == slotPositionSource.Z );
                    entity.SetFormationSlot( slotPositionTarget );
                }
                // swap position with entity of other type
                else if ( dragDropTarget != null && dragDropTarget.GetEntityType() != dragDropSource.GetEntityType() )
                {
                    var slotPositionTarget     = dragDropTarget.SlotPosition;
                    var gridSlotObjectTarget   = GetGridSlotObject( slotPositionTarget );
                    var positionTarget         = gridSlotObjectTarget.GetComponent< Transform >().position;
                    var slotPositionSource     = dragDropSource.SlotPosition;
                    var gridSlotObjectSource   = GetGridSlotObject( slotPositionSource );
                    var positionSource         = gridSlotObjectSource.GetComponent< Transform >().position;
                    
                    var player                 = Finder.GetPlayer();
                    var formationConfiguration = player.GetComponent< FormationConfiguration >();
                    var entities               = formationConfiguration.GetUnderlingUnits();

                    // search entities before writing positions
                    var sourceEntity           = entities.Single( a_x => a_x.GetFormationSlot().X == slotPositionSource.X && a_x.GetFormationSlot().Z == slotPositionSource.Z );
                    var targetEntity           = entities.Single( a_x => a_x.GetFormationSlot().X == slotPositionTarget.X && a_x.GetFormationSlot().Z == slotPositionTarget.Z );
                    
                    // set target position to drop source
                    dragDropSource.GetComponent< FormationEditorDragDropTarget >().SlotPosition = slotPositionTarget;
                    // snap to grid position of drop target
                    dragDropSource.GetComponent< Transform >().position = positionTarget;
                    // write target slot position to source non-proxy entity
                    sourceEntity.SetFormationSlot( slotPositionTarget );
                    // set source position to drop target
                    dragDropTarget.GetComponent< FormationEditorDragDropTarget >().SlotPosition = slotPositionSource;
                    // snap to grid position of drop source
                    dragDropTarget.GetComponent< Transform >().position = positionSource;
                    // write source slot position to target non-proxy entity
                    targetEntity.SetFormationSlot( slotPositionSource );
                }
                else // reset to original position
                {
                    var slotPosition     = dragDropSource.SlotPosition;
                    var gridSlotObject   = GetGridSlotObject( slotPosition );
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
        // re-activate all entities
        foreach ( var entity in Finder.GetEntityManager().GetUnderlings() )
        {
            if ( entity.GetLeader() == Finder.GetPlayer() )
            {
                entity.gameObject.SetActive( true );
            }
            else // only render enemies -> deactivate all behaviors
            {
                foreach ( var component in entity.GetComponents< Behaviour >() )
                {
                    component.enabled = true;
                }
            }
        }
        foreach (var entity in Finder.GetEntityManager().GetDeadUnderlings() )
        {
            entity.gameObject.SetActive( true );
        }

        // delete proxies
        foreach ( var proxyObject in m_proxyObjects )
        {
            Object.Destroy( proxyObject );
        }
        m_proxyObjects = null;

        // delete grid slot objects
        foreach ( var slotObject in m_gridSlotObjects )
        {
            Object.Destroy( slotObject );
        }
        m_gridSlotObjects = null;

        var cameraOrbitScript = Finder.GetCameraOrbitScript();

        Finder.GetOrthoPerspectiveSwitcher().SwitchToPerspective();
        cameraOrbitScript.AreControlsEnabled = true;
        cameraOrbitScript.LocalRotation      = m_originalOrbitRotation;
        
        // update formation collider size
        var formationBoundsUpdater = Finder.GetPlayer().GetComponentInChildren< FormationBoundsUpdater >();
        formationBoundsUpdater.UpdateFormationBoundingBox();

        base.Exit();
    }

    private static FormationEditorDragDropTarget GetGridSlotDragDropTarget( GameObject a_gridSlot )
    {
        return a_gridSlot.GetComponent< FormationEditorDragDropTarget >();
    }

    private GameObject GetGridSlotObject( Position2 a_position )
    {
        return m_gridSlotObjects.SingleOrDefault( a_x => AreEqual( GetGridSlotDragDropTarget( a_x ).SlotPosition, a_position ) );
    }

    private static bool AreEqual( Position2 a_one, Position2 a_other )
    {
        return a_one.X == a_other.X && a_one.Z == a_other.Z;
    }
}
