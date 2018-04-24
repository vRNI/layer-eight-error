using JetBrains.Annotations;
using UnityEngine;

[ DisallowMultipleComponent ]
public class PlayerStateManager
    : MonoBehaviour
{
    private PlayerState m_currentState;
    
    /// <summary>
    /// Gets the currently active player state.
    /// </summary>
    [ NotNull ]
    public PlayerState GetCurrentState()
    {
        return m_currentState;
    }
    
    /// <summary>
    /// Sets the currently active player state.
    /// </summary>
    public void SetCurrentState< TState >()
        where TState : PlayerState, new()
    {
        m_currentState.Exit();
        m_currentState = new TState();
        m_currentState.SetPlayer( Finder.GetPlayer() );
        m_currentState.Enter();
    }

    private void Start ()
    {
        // Idle state by default.
        m_currentState = new IdlePlayerState();
        m_currentState.SetPlayer( Finder.GetPlayer() );
        m_currentState.Enter();
    }
    
    private void Update ()
    {
        m_currentState.Update();
    }
}
