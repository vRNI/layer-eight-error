using UnityEngine;

public abstract class GameState
{
    public virtual void Entering()
    {
    }

    public abstract void Update();

    public virtual void Exiting()
    {
    }
}
