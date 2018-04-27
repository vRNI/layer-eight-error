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

    [ SerializeField ]
    private GameObject m_failStateHeading;

    public GameObject InstantiateFailStateHeading()
    {
        return Object.Instantiate( m_failStateHeading );
    }

    [ SerializeField ]
    private GameObject m_failStateTitle;
    
    public GameObject InstantiateFailStateTitle()
    {
        return Object.Instantiate( m_failStateTitle );
    }
    
    [ SerializeField ]
    private GameObject m_failStateRestart;
    
    public GameObject InstantiateFailStateRestart()
    {
        return Object.Instantiate( m_failStateRestart );
    }
    
    [ SerializeField ]
    private GameObject m_failStateExit;
    
    public GameObject InstantiateFailStateExit()
    {
        return Object.Instantiate( m_failStateExit );
    }
    
    [ SerializeField ]
    private GameObject m_failStateUs;
    
    public GameObject InstantiateFailStateUs()
    {
        return Object.Instantiate( m_failStateUs );
    }
    
    [ SerializeField ]
    private GameObject m_flag;
    
    public GameObject InstantiateFlag()
    {
        return Object.Instantiate( m_flag );
    }
}
