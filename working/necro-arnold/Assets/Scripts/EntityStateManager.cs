using UnityEngine;
using System;
using JetBrains.Annotations;

[DisallowMultipleComponent]
public class EntityStateManager 
    : MonoBehaviour {

    private EntityState m_currentState;

    /// <summary>
    /// Gets the currently active entity state.
    /// </summary>
    [NotNull]
    public EntityState GetCurrentState()
    {
        return m_currentState;
    }

    /// <summary>
    /// Sets the currently active entity state.
    /// </summary>
    public void SetCurrentState<TState>()
        where TState : EntityState, new()
    {
        m_currentState.Exiting(); ;
        m_currentState = new TState();
        m_currentState.Entering();
    }

    // Use this for pre-initialization
    private void Awake()
    {
        // Set the manager node tag to the owning entity object.
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
