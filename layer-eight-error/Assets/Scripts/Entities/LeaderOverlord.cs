
public class LeaderOverlord
    : LeaderEntity
{
    public override bool IsDead()
    {
        return m_healthPoints <= 0;
    }

    protected override void Die()
    {
        // change to fail state
        if ( Finder.GetGameStateManager().IsCurrentState< FailGameState >() == false )
        {
            Finder.GetGameStateManager().GetCurrentState().TriggerTransition< FailGameState >();
        }
    }
}
