using UnityEngine;

public class PrefabManager
    : MonoBehaviour
{
    [ SerializeField ]
    private GameObject m_formationSlotPlacement;

    public GameObject GetFormationSlotPlacement()
    {
        return Object.Instantiate( m_formationSlotPlacement );
    }

    [ SerializeField ]
    private GameObject m_formationFreeFighterUnits;

    public GameObject GetFormationFreeFighterUnits()
    {
        return Object.Instantiate( m_formationFreeFighterUnits );
    }

    [ SerializeField ]
    private GameObject m_formationFreeRangerUnits;
    
    public GameObject GetFormationFreeRangerUnits()
    {
        return Object.Instantiate( m_formationFreeRangerUnits );
    }

    [ SerializeField ]
    private GameObject m_formationFreeMageUnits;
    
    public GameObject GetFormationFreeMageUnits()
    {
        return Object.Instantiate( m_formationFreeMageUnits );
    }

    // Use this for pre-initialization
    private void Awake()
    {
        if ( m_formationSlotPlacement == null ) { ValidityManager.TerminateUnexpectedly( "The formation slot placement prefab is not defined, but it is required." ); }
        if ( m_formationFreeFighterUnits == null ) { ValidityManager.TerminateUnexpectedly( "The formation free fighter units prefab is not defined, but it is required." ); }
        if ( m_formationFreeRangerUnits == null ) { ValidityManager.TerminateUnexpectedly( "The formation free ranger units placement prefab is not defined, but it is required." ); }
        if ( m_formationFreeMageUnits == null ) { ValidityManager.TerminateUnexpectedly( "The formation free mage units prefab is not defined, but it is required." ); }
    }
}
