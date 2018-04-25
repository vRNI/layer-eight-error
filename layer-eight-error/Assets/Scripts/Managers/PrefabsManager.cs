using UnityEngine;

public class PrefabsManager
    : MonoBehaviour
{
    [ SerializeField ]
    private EntityProxyInfo[] m_entityProxies;

    public EntityProxyInfo[] GetEntityProxies()
    {
        return m_entityProxies;
    }
}
