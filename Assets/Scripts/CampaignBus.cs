﻿using UnityEngine;
using UnityEngine.Assertions;

public class CampaignBus : MonoBehaviour
{
    private iTweenPath _busPathScript;
    private float _completedPathPercentage;
    private Vector3[] _pathNodesArray;
    private float _movementSpeedForCurrentNode;
    private int _nextNodeIndex;

    public float MovementSpeed;
    public float LookAheadAmount;

    public Camera gameCam;
    
	void Start ()
	{
        _busPathScript = GameObject.FindGameObjectWithTag("BusPath").GetComponent<iTweenPath>();
        _pathNodesArray = _busPathScript.nodes.ToArray();
	    _completedPathPercentage = 0.0f;
	    _movementSpeedForCurrentNode = 0.0f;
	    _nextNodeIndex = 0;
	}
	
	void FixedUpdate ()
	{
	    _completedPathPercentage %= 1.0f;

	    int potentialNextNodeIndex = FindNextNodeIndex();

	    if (_nextNodeIndex != potentialNextNodeIndex)
	    {
	        _nextNodeIndex = potentialNextNodeIndex;
            Vector3[] subPath = { _pathNodesArray[_nextNodeIndex - 1], _pathNodesArray[_nextNodeIndex] };
	        _movementSpeedForCurrentNode = MovementSpeed/iTween.PathLength(subPath);
        }
	    _completedPathPercentage += _movementSpeedForCurrentNode*Time.fixedDeltaTime;

        //Vector3 prevPos = transform.position;

        iTween.PutOnPath(gameObject, _pathNodesArray, _completedPathPercentage);

        Vector3 lookTarget = iTween.PointOnPath(_pathNodesArray, _completedPathPercentage + LookAheadAmount);
        
        transform.LookAt(lookTarget);

        //print(Vector3.Distance(prevPos, transform.position));
	}

    int FindNextNodeIndex() //TODO_ARHAN_LATER don't always calculate if possible
    {
        int pathLineCount = _pathNodesArray.Length - 1;
        Assert.IsTrue(pathLineCount > 0);
        float pathPercentageIncrement = 1.0f/pathLineCount;

        for (int i = 0; i < _pathNodesArray.Length; ++i)
        {
            if (_completedPathPercentage < pathPercentageIncrement*i)
            {
                //print(i);
                return i;
            }
        }
        Assert.IsTrue(false); //we should not come here! if we're here, something's broken
        return -1; 
    }
}
