using UnityEngine;

public class Fighter
    : Entity
{

    public override void Update()
    {
        base.Update();
    }

    public override void Awake()
    {
        base.Awake();

        m_healthPoints = 150;
        m_damagePoints = 20;
    }
}