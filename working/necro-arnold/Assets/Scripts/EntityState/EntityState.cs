using UnityEngine;

public abstract class EntityState
{    public virtual void Entering()
    {
    }
    public abstract void Update();

    public virtual void Exiting()
    {
    }
}