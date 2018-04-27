using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour {

    [SerializeField]
    private bool m_collisionDetected;

    [SerializeField]
    bool m_isPlayer;
    [SerializeField]
    List<Position2> m_slotPosition;
    [SerializeField]
    List<EntityType> m_correspondingEntityTypes;
    [SerializeField]
    GameObject m_leaderEntity;
    [SerializeField]
    GameObject m_detector;
    [SerializeField]
    GameObject[] m_prefabs;

    // listener -> fire object

	// Use this for initialization
	void Start () {
        Finder.GetSpawnManager().RegisterSpawner(this);
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    private void FixedUpdate()
    {
        m_collisionDetected = false;
    }

    private void Awake()
    {
        
    }

    public void SpawnFormation()
    {
        if (m_slotPosition.Count != m_correspondingEntityTypes.Count)
        {
            throw new System.InvalidOperationException("Slot count must be equal to corresponding entity count.");
        }

        m_leaderEntity = Instantiate(m_leaderEntity, transform.position, transform.rotation);
        m_detector = Instantiate(m_detector, transform.position, transform.rotation);

        m_detector.GetComponent<FormationBoundsUpdater>().SetFormationLeader(m_leaderEntity);
        m_detector.GetComponent<FollowSquad>().SetFormationLeader(m_leaderEntity);

        Vector3 sourcePostion = transform.position; //The position you want to place your agent
        NavMeshHit closestHit;
        NavMesh.SamplePosition(sourcePostion, out closestHit, 500, 1);

        for (int i = 0; i < m_slotPosition.Count; i++)
        {
            GameObject underling = null;
            switch (m_correspondingEntityTypes[i])
            {
                case (EntityType.Fighter):
                    underling = Instantiate(m_prefabs[0], transform.position, transform.rotation);
                    break;
                case (EntityType.Ranger):
                    underling = Instantiate(m_prefabs[1], transform.position, transform.rotation);
                    break;
                case (EntityType.Mage):
                    underling = Instantiate(m_prefabs[2], transform.position, transform.rotation);
                    break;
                case (EntityType.None):
                    break;
            }

            // if slot was not none
            if (underling != null)
            {
                var entityComponent = underling.GetComponent<UnderlingEntity>();
                if (entityComponent != null)
                {
                    entityComponent.SetLeader(m_leaderEntity);
                    entityComponent.SetFormationSlot(m_slotPosition[i]);
                    if (m_isPlayer)
                        entityComponent.IsFriendly = true;
                }
            }
        }

        m_detector.GetComponent<FormationBoundsUpdater>().UpdateFormationBoundingBox();
    }

    public bool IsEntityInSpawner()
    {
        return m_collisionDetected;
    }

    public void OnTriggerStay(Collider other)
    {
        // TODO: change this shiat
        var followSquadComponent = other.GetComponent<FollowSquad>();
        if (followSquadComponent != null)
            m_collisionDetected = true;
    }
}
