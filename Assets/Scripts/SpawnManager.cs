using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public int PedestrianSpawnCount;
    public GameObject PedestrianPrefab;
    
	void Start ()
    {
		//attached gameobject will spawn new pedestrians within limits, where y = 0 and x,z is determined randomly

	    float randomXLim = transform.localScale.x*0.5f;
	    float randomZLim = transform.localScale.z*0.5f;

        transform.localScale = Vector3.one; //you dirty bastard
        for (int i = 0; i < PedestrianSpawnCount; ++i)
	    {
            Vector3 newSpawnPos = transform.TransformPoint(new Vector3(Random.Range(-randomXLim, randomXLim), 0,
                Random.Range(-randomZLim, randomZLim)));
            
	        Instantiate(PedestrianPrefab, newSpawnPos, Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.up));
        }
    }
}
