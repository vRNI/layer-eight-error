using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Switches camera projection between orthographic and perspective.
/// </summary>
[ RequireComponent( typeof( Camera ) ) ]
public class OrthoPerspectiveSwitcher
    : MonoBehaviour
{
    private enum NextSwitcherState
    {
        None,
        Perspective,
        Ortho,
    }

    private Matrix4x4 m_ortho;
    private Matrix4x4 m_perspective;

    [ SerializeField ]
    private float fov              =   60.0f;
    [ SerializeField ]
    private float near             =    0.3f;
    [ SerializeField ]
    private float far              = 1000.0f;
    //[ SerializeField ]
    private float orthographicSize =   10.0f;
 
    private NextSwitcherState m_nextState = NextSwitcherState.Perspective;
 
    private void Update()
    {
        var player                 = Finder.GetPlayer();
        var formationConfiguration = player.GetComponent< FormationConfiguration >();

        var gridWidth              = formationConfiguration.GetSlotDistance().x * formationConfiguration.GetGridSize().X;
        var gridHeight             = formationConfiguration.GetSlotDistance().y * formationConfiguration.GetGridSize().Z;

        // divide grid size by two, because of left = -orthographicSize and right = +orthographicSize
        orthographicSize = Mathf.Max( gridHeight / 2.0f, gridWidth / 2.0f );
        
        var aspect = ( Screen.width + 0.0f ) / ( Screen.height + 0.0f );
 
        m_perspective = Matrix4x4.Perspective( fov, aspect, near, far );
        m_ortho       = Matrix4x4.Ortho( -orthographicSize * aspect, orthographicSize * aspect, -orthographicSize, orthographicSize, near, far );

        // check if state has changed, update matrix accordingly
        switch ( m_nextState )
        {
            case NextSwitcherState.None:
                break;
            case NextSwitcherState.Perspective:
                BlendToMatrix( m_perspective, 1f );
                break;
            case NextSwitcherState.Ortho:
                BlendToMatrix( m_ortho, 1f );
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        // after starting co-routine wait until next state change is requested
        m_nextState = NextSwitcherState.None;
    }
 
    private static Matrix4x4 MatrixLerp( Matrix4x4 a_from, Matrix4x4 a_to, float a_time )
    {
        var ret = new Matrix4x4();
        for ( int i = 0; i < 16; i++ )
        {
            ret[ i ] = Mathf.Lerp( a_from[ i ], a_to[ i ], a_time );
        }

        return ret;
    }
 
    private IEnumerator LerpFromTo( Matrix4x4 a_src, Matrix4x4 a_dest, float a_duration )
    {
        var startTime = Time.time;
        while ( Time.time - startTime < a_duration )
        {
            gameObject.GetComponent< Camera >().projectionMatrix = MatrixLerp( a_src, a_dest, ( Time.time - startTime ) / a_duration );
        
            yield return 1;
        }

        gameObject.GetComponent< Camera >().projectionMatrix = a_dest;
    }
 
    public Coroutine BlendToMatrix( Matrix4x4 a_targetMatrix, float a_duration )
    {
        StopAllCoroutines();

        return StartCoroutine( LerpFromTo( gameObject.GetComponent< Camera >().projectionMatrix, a_targetMatrix, a_duration ) );
    }

    /// <summary>
    /// Tells the camera to switch to perspective view.
    /// </summary>
    public void SwitchToPerspective()
    {
        m_nextState = NextSwitcherState.Perspective;
    }
    
    /// <summary>
    /// Tells the camera to switch to orthographic view.
    /// </summary>
    public void SwitchToOrtho()
    {
        m_nextState = NextSwitcherState.Ortho;
    }
}
