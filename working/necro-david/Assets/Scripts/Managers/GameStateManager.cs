using JetBrains.Annotations;
using UnityEngine;

[ DisallowMultipleComponent ]
public class GameStateManager 
    : MonoBehaviour
{
    private GameState m_currentState;
    
    /// <summary>
    /// Gets the currently active game state.
    /// </summary>
    [ NotNull ]
    public GameState GetCurrentState()
    {
        return m_currentState;
    }
    
    /// <summary>
    /// Sets the currently active game state.
    /// </summary>
    public void SetCurrentState< TState >()
        where TState : GameState, new()
    {
        m_currentState.Exit();;
        m_currentState = new TState();
        m_currentState.Enter();
    }

    private void Start ()
    {
        // Idle state by default.
        m_currentState = new IdleGameState();
        m_currentState.Enter();
    }
    
    private void Update ()
    {
        m_currentState.Update();
    }
}
