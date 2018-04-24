
using UnityEngine;

public class FormationGameState
    : GameState
{
    private Vector3 m_originalOrbitRotation;

    public override void Enter()
    {
        base.Enter();

        var cameraOrbitScript   = Finder.GetCameraOrbitScript();
        m_originalOrbitRotation = cameraOrbitScript.LocalRotation;
        
        cameraOrbitScript.LocalRotation      = new Vector3( m_originalOrbitRotation.x, 90.0f, m_originalOrbitRotation.z );
        cameraOrbitScript.AreControlsEnabled = false;
        Finder.GetOrthoPerspectiveSwitcher().SwitchToOrtho();
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
        var cameraOrbitScript = Finder.GetCameraOrbitScript();

        Finder.GetOrthoPerspectiveSwitcher().SwitchToPerspective();
        cameraOrbitScript.AreControlsEnabled = true;
        cameraOrbitScript.LocalRotation      = m_originalOrbitRotation;

        base.Exit();
    }
}
