using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabsManager
    : MonoBehaviour
{
    [ SerializeField ]
    private GameObject m_emptyFormationSlot;

    public GameObject GetEmptyFormationSlot()
    {
        return m_emptyFormationSlot;
    }

    [ SerializeField ]
    private EntityProxyInfo[] m_entityProxies;

    public EntityProxyInfo[] GetEntityProxies()
    {
        return m_entityProxies;
    }
}
