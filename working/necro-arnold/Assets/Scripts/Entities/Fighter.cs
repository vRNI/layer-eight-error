using UnityEngine;

public abstract class Fighter
    : Entity
{

    public override void Update()
    {

    }

    public override void Awake()
    {
        m_healthPoints = 150;
        m_damagePoints = 20;
    }

}