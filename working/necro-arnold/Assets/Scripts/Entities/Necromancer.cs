using UnityEngine;

public class Necromancer 
    : Entity {


    public override void Update()
    {
        base.Update();
    }

    public override void Awake()
    {
        base.Awake();
        m_healthPoints = 500;
        m_damagePoints = 100;
    }

}