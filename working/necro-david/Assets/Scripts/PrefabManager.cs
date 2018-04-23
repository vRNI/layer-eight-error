using UnityEngine;

public class PrefabManager
    : MonoBehaviour
{
    [SerializeField]
    private GameObject m_formationSlotPlacement;

    public GameObject GetFormationSlotPlacement()
    {
        return Object.Instantiate( m_formationSlotPlacement );
    }

    // Use this for pre-initialization
    private void Awake()
    {
        if ( m_formationSlotPlacement == null ) { ValidityManager.TerminateUnexpectedly( "The formation slot placement prefab is not defined, but it is required." ); }
    }
}
