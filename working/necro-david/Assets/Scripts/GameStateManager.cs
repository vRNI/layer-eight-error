using UnityEngine;
using System;
using JetBrains.Annotations;

[DisallowMultipleComponent]
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
    /// <param name="a_currentState">
    /// The game state to set.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="a_currentState"/> is null.
    /// </exception>
    public void SetCurrentState( [ NotNull ] GameState a_currentState )
    {
        if ( a_currentState == null ) { throw new ArgumentNullException( "a_currentState" ); }

        m_currentState = a_currentState;
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
    }

    // Update is called once per frame
    private void Update()
    {
        m_currentState.Update();
    }
}
