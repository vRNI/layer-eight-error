
public abstract class EntityState
{
    protected UnderlingEntity m_entity;

    /// <summary>
    /// Sets the entity this state belongs to.
    /// </summary>
    /// <param name="a_entity">
    /// The entity this state belongs to.
    /// </param>
    public void SetEntity(UnderlingEntity a_entity)
    {
        m_entity = a_entity;
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