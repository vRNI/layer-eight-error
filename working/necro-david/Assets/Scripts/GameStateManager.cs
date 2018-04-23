using UnityEngine;
using System;
using JetBrains.Annotations;

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
        m_currentState.Exiting();;
        m_currentState = new TState();
        m_currentState.Entering();
    }

    // Use this for pre-initialization
    private void Awake()
    {
        // Set the manager node tag to the owning game object.
        gameObject.tag = TagName.ManagerNode;
    }

    // Use this for initialization
    private void Start()
    {
        // Idle state by default.
        m_currentState = new IdleState();
        m_currentState.Entering();
    }

    // Update is called once per frame
    private void Update()
    {
        m_currentState.Update();
    }
}
