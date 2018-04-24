
using System.Linq;
using UnityEngine;

public class FormationGameState
    : GameState
{
    private Vector3 m_originalOrbitRotation;
    private GameObject[] m_gridSlotObjects;

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
        
        int index = -1;
        foreach ( var slotPosition in formationConfiguration.EnumerateSlotPosition() )
        {
            m_gridSlotObjects[ ++index ] = Object.Instantiate( prefabsManager.GetEmptyFormationSlot() );
            m_gridSlotObjects[ index ].GetComponent< FormationEditorDragDropTarget >().SlotPosition = slotPosition;

            var slotOffset             = formationConfiguration.GetSlotOffset( slotPosition );
            var slotOffsetRotated      = Quaternion.AngleAxis( playerRotationY, Vector3.up ) * slotOffset;
            var targetPosition         = playerPosition + slotOffsetRotated + new Vector3( 0.0f, 2.0f, 0.0f ); // move stuff a bit upwards

            m_gridSlotObjects[ index ].GetComponent< Transform >().position   = targetPosition;
            m_gridSlotObjects[ index ].GetComponent< Transform >().localScale = new Vector3( 0.3f, 0.3f, 0.3f );
        }

        foreach ( var entity in entities )
        {
            var slotPosition   = entity.GetFormationSlot();
            var gridSlotObject = m_gridSlotObjects.SingleOrDefault( a_x => AreEqual( a_x.GetComponent< FormationEditorDragDropTarget >().SlotPosition, slotPosition ) );
            if ( gridSlotObject == null ) { continue; }
            gridSlotObject.SetActive( false );
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
