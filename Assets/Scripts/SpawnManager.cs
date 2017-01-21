using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public int PedestrianSpawnCount;
    public GameObject[] PedestrianPrefabs;
    
	void Start ()
    {
        // update pedestrian count
        CameraScript cameraScript = Camera.main.GetComponent<CameraScript>();
        cameraScript.AddPedestrians(PedestrianSpawnCount);
        
		//attached gameobject will spawn new pedestrians within limits, where y = 0 and x,z is determined randomly

	    float randomXLim = transform.localScale.x*0.5f;
	    float randomZLim = transform.localScale.z*0.5f;

        transform.localScale = Vector3.one; //you dirty bastard
        for (int i = 0; i < PedestrianSpawnCount; ++i)
	    {
            Vector3 newSpawnPos = transform.TransformPoint(new Vector3(Random.Range(-randomXLim, randomXLim), -1f,
                Random.Range(-randomZLim, randomZLim)));

	        int randomPedestrianIndex = Random.Range(1, 10);
	        int arrayIndex = 2;
	        if (randomPedestrianIndex < 5)
	        {
	            arrayIndex = 1;
	        }
            else if (randomPedestrianIndex < 9)
            {
                arrayIndex = 0;
            }
	        Instantiate(PedestrianPrefabs[arrayIndex], newSpawnPos, Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.up));
        }
    }
}
