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
    private GameObject[] m_allies;

    public GameObject[] GetAllies()
    {
        return m_allies;
    }
}
