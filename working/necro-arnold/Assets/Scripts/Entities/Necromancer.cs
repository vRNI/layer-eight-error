using UnityEngine;

public abstract class Necromancer 
    : Entity {

    public override void Update()
    {

    }

    public override void Awake()
    {
        m_healthPoints = 500;
        m_damagePoints = 100;
    }

}