
using UnityEngine;

public abstract class PlayerState
{
    protected GameObject m_player;
    
    /// <summary>
    /// Sets the player this state belongs to.
    /// </summary>
    /// <param name="a_player">
    /// The player this state belongs to.
    /// </param>
    public void SetPlayer( GameObject a_player )
    {
        m_player = a_player;
    }

    public virtual void Enter()
    {
    }

    public virtual void Update()
    {
    }

    public virtual void Exit()
    {
    }
}
