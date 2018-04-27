using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EntityManager : MonoBehaviour {

    List<UnderlingEntity> underlingEntities = new List<UnderlingEntity>();
    List<UnderlingEntity> deadUnderlingEntities = new List<UnderlingEntity>();

    private void Start()
    {
        
    }

    private void Update()
    {
        //
    }
    
    public void AddUnderling(UnderlingEntity a)
    {
        underlingEntities.Add(a);
    }

    public List<UnderlingEntity> GetUnderlings()
    {
        return underlingEntities;
    }

    public List<UnderlingEntity> GetDeadUnderlings()
    {
        return deadUnderlingEntities;
    }

    public void RemoveUnderling(UnderlingEntity a)
    {
        underlingEntities.Remove(a);
    }

    public void PushToDeadUnderlings(UnderlingEntity a)
    {
        underlingEntities.Remove(a);
        deadUnderlingEntities.Add(a);
    }

    public void RemoveDeadUnderling(UnderlingEntity a)
    {
        deadUnderlingEntities.Remove(a);
    }
}
