
public abstract class EntityState
{
    protected Entity m_entity;

    public void SetEntity( Entity a_entity )
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