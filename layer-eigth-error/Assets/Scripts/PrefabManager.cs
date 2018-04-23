using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Runtime;
using UnityEngine;

[ DisallowMultipleComponent ]
public class PrefabManager
    : MonoBehaviour
{
    [ SerializeField ]
    private GameObject m_formationSlot;

    public GameObject GetFormationSlot()
    {
        return Object.Instantiate( m_formationSlot );
    }

    [ SerializeField ]
    private GameObject m_formationFreeFighterUnitCounter;
    
    public GameObject GetFreeFighterUnitCounter()
    {
        return Object.Instantiate( m_formationFreeFighterUnitCounter );
    }

    public void Awake ()
    {
        //if ( m_formationSlot == null ) { throw new RuntimeException( "Prefab: Formation Slot is null." );}
        //if ( m_formationFreeFighterUnitCounter == null ) { throw new RuntimeException( "Prefab: Formation Slot is null." );}
    }
}
