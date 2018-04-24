using UnityEngine;
using JetBrains.Annotations;

[DisallowMultipleComponent]
public class EntityStateManager 
    : MonoBehaviour
{
    private EntityState m_currentState;
    private Entity      m_entity;

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
        if ( m_currentState != null ) { m_currentState.Exit(); }

        m_currentState = new TState();
        m_currentState.SetEntity( m_entity );
        m_currentState.Enter();
    }
    
    /// <summary>
    /// Sets the entity this state manager belongs to.
    /// </summary>
    /// <param name="a_entity">
    /// The entity this state manager belongs to.
    /// </param>
    public void SetEntity( Entity a_entity )
    {
        m_entity = a_entity;
    }
    
    private void Update()
    {
        if ( m_currentState != null ) { m_currentState.Update(); }
    }
}
