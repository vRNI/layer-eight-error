
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
        
        cameraOrbitScript.LocalRotation      = new Vector3( m_originalOrbitRotation.x, 90.0f, m_originalOrbitRotation.z );
        cameraOrbitScript.AreControlsEnabled = false;
        Finder.GetOrthoPerspectiveSwitcher().SwitchToOrtho();

        var player                 = Finder.GetPlayer();
        var playerPosition         = player.GetComponent< Transform >().position;
        var playerRotationY        = player.GetComponent< Transform >().eulerAngles.y;
        var formationConfiguration = player.GetComponent< FormationConfiguration >();
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
