using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public int PedestrianSpawnCount;
    public GameObject[] PedestrianPrefabs;
    public GameObject BusMoverGameObject;

    private List<Pedestrian> _createdPedestrians;
    private AudioSource _zoneAudioSource;
    private bool _screamsPlayed;
    private Vector3 _spawnZoneScale;
    private CameraScript _cameraScript;
    private Vector3[] _busPath;
    

    
	void Start ()
	{
	    _screamsPlayed = false;
	    _zoneAudioSource = gameObject.GetComponent<AudioSource>();
        _cameraScript = Camera.main.GetComponent<CameraScript>();

        //attached gameobject will spawn new pedestrians within limits, where y = 0 and x,z is determined randomly
        _spawnZoneScale = transform.localScale;
        transform.localScale = Vector3.one; //you dirty bastard
        
        _createdPedestrians = new List<Pedestrian>();

	    if (BusMoverGameObject == null)
	    {
            BusMoverGameObject = GameObject.FindGameObjectWithTag("BusMover");
        }
	    _busPath = BusMoverGameObject.GetComponentInParent<iTweenPath>().nodes.ToArray();

#if UNITY_WEBGL
	    PedestrianSpawnCount = Mathf.FloorToInt(PedestrianSpawnCount * 0.4f);
#endif

	    for (int i = 0; i < PedestrianSpawnCount; ++i)
	    {
            float randomXLim = _spawnZoneScale.x * 0.5f;
            float randomZLim = _spawnZoneScale.z * 0.5f;

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

            GameObject tempPedestrianGameObject = Instantiate(PedestrianPrefabs[arrayIndex], newSpawnPos,
                Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.up));
            Pedestrian tempPedestrian = tempPedestrianGameObject.GetComponent<Pedestrian>();
            tempPedestrian.Initialize(_busPath, BusMoverGameObject);

            _createdPedestrians.Add(tempPedestrian);
        }
        _cameraScript.AddPedestrians(PedestrianSpawnCount);
    }

    void Update()
    {
        if (!_screamsPlayed)
        {
            int count = 0;
            foreach (Pedestrian p in _createdPedestrians)
            {
                if (p.GetMoveState() == Pedestrian.MoveState.MsLoveRun || p.GetIsImpressed())
                {
                    ++count;
                }
            }

            if (count >= 0.25f * _createdPedestrians.Count)
            {
                _screamsPlayed = true;
                _zoneAudioSource.Play();
            }
        }
    }
}
