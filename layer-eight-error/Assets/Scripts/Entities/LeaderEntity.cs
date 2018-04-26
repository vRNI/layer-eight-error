using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(FormationConfiguration))]
public class LeaderEntity : BaseEntity {

    protected FormationConfiguration m_formationConfiguration;

    public FormationConfiguration p_formationConfiguration
    {
        // We edit the formation configuration directly
        get { return m_formationConfiguration; }
        private set { }
    }

    public override bool IsDead()
    {
        return m_formationConfiguration.GetUnderlingUnits().All( a_x => a_x.IsDead() );
    }
    
    protected override void Awake()
    {
        base.Awake();

        m_formationConfiguration = gameObject.GetComponent<FormationConfiguration>();
    }

    public FormationConfiguration GetFormationConfiguration()
    {
        return m_formationConfiguration;
    }

    public void AddUnderlingUnit(UnderlingEntity a)
    {
        m_formationConfiguration.AddUnderlingEntity(a);
    }

    public void SetUnderlingsState<TState>()
        where TState : EntityState, new()
    {
        var underlings = m_formationConfiguration.GetUnderlingUnits();
        foreach (var underling in underlings)
        {
            underling.SetCurrentState<TState>();
        }
    }

    public void SetUnderlingsHostility(bool hostility)
    {
        m_isHostile = hostility;
        var underlings = m_formationConfiguration.GetUnderlingUnits();
        foreach (var underling in underlings)
        {
            underling.IsHostile = hostility;
        }
    }
}
