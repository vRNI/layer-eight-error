using System.Collections;
using System.Collections.Generic;
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

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Awake()
    {
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
