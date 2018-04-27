using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    float m_timer;
    [SerializeField]
    float m_timerIntervall;

    List<Spawner> m_spawners = new List<Spawner>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        m_timer += Time.deltaTime;

        if (m_timer >= m_timerIntervall)
        {
            var spawner = GetAvailableSpawner();
            if (spawner != null)
            {
                spawner.SpawnFormation();
                m_timer = 0;
            }
        }
	}

    Spawner GetAvailableSpawner()
    {
        var player = Finder.GetPlayer();
        Spawner spawner = null;
        float farthestDistance = float.MinValue;
        float currentDistance = float.MinValue;
        foreach(var s in m_spawners)
        {
            currentDistance = Vector3.Distance(player.transform.position, s.gameObject.transform.position);
            if (currentDistance > farthestDistance && !s.IsEntityInSpawner())
                spawner = s;
        }

        return spawner;
    }

    public void RegisterSpawner(Spawner a_spawner)
    {
        m_spawners.Add(a_spawner);
    }

    // dont need to remove the spawner, yet
}
