using UnityEngine;

public abstract class GameState
{
    public virtual void Enter()
    {
    }

    public virtual void Update()
    {
    }

    public virtual void Exit()
    {
    }

    public virtual void TriggerTransition<TState>()
        where TState : GameState, new()
    {
        var gameStateManager = Finder.GetGameStateManager();
        gameStateManager.SetCurrentState<TState>();
        Debug.Log(typeof(TState).FullName);
    }
}