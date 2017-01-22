using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Pedestrian : MonoBehaviour
{
    public float MinDistanceFromBusPath;
    public float PositionRelaxCoef;
    public float MinMovementDistance;
    public float MaxMovementDistance;
    public float MinIdleDuration;
    public float MaxIdleDuration;
    public float RegularSpeed;
    public float LoveRunSpeed;
    public float RotationSpeed;
    public float LoveRunTriggerRange;

    [Range(0.0f, 1.0f)]
    public float LovePresidentPossibility;
    
    [Range(0.2f, 2.0f)]
    public float SecondsRequiredToImpress; //TODO_ARHAN implement
    [Range(0, 10000)]
    public int GiveUpCoef;

    private Vector3 _targetPointOnSpline;
    private bool _lovesPresident;
    private bool _loveRunTriggered;
    private iTweenPath _busPathScript;
    private Vector3[] _busPath;
    private float _nextWanderTime;
    private Vector3 _wanderDirection;
    private Vector3 _nextWanderPos;
    private Vector3 _posBeforeLoveRun;
    private float _actualSpeed;
    private float _prevDistanceFromTargetPoint;
    private float _remainingSecondsToImpress;
    private Renderer _bodyRenderer;

    private GameObject _busMoverGameObject;

    private bool _isImpressed; //TODO_ARHAN goal of the game is to make as much pedestrians impressed as possible

    public enum MoveState
    {
        MsIdle,
        MsWandering,
        MsLoveRun
    }

    private MoveState _moveState;

	// Use this for initialization
	void Start ()
    {
        _moveState = MoveState.MsIdle;
	    _actualSpeed = RegularSpeed;
        _nextWanderTime = Time.time;
        _nextWanderPos = Vector3.zero;
	    _posBeforeLoveRun = Vector3.zero;
        _wanderDirection = Vector3.zero;
        _isImpressed = false;

        Renderer[] childRenderers = gameObject.GetComponentsInChildren<Renderer>();

        foreach (Renderer childRenderer in childRenderers)
        {
            if (childRenderer.gameObject.tag == "body")
            {
                _bodyRenderer = childRenderer;
            }
        }

        //she loves me, she loves me not
        _lovesPresident = (Random.Range(0.0f, 1.0f) < LovePresidentPossibility);
        _loveRunTriggered = false;

        _busMoverGameObject = GameObject.FindGameObjectWithTag("BusMover");
	    _prevDistanceFromTargetPoint = float.MaxValue;
        
	    _remainingSecondsToImpress = SecondsRequiredToImpress;
	    if (_lovesPresident)
	    {
	        _remainingSecondsToImpress *= .5f;
            SetBodyColor();
	    }

        //determine target point on spline, first get bus path, then you'll do stuff
        _busPathScript = GameObject.FindGameObjectWithTag("BusPath").GetComponent<iTweenPath>();
        _busPath = _busPathScript.nodes.ToArray();
        SetTargetPointOnSpline();
        //print("target: " + _targetPointOnSpline);
    }
	
	// Update is called once per frame
	void Update ()
	{
	    bool busIsApproaching = false;
        float distanceFromTargetPoint = Vector3.Distance(_busMoverGameObject.transform.position, _targetPointOnSpline);
	    if (_prevDistanceFromTargetPoint > distanceFromTargetPoint)
	    {
	        busIsApproaching = true;
	    }
        _prevDistanceFromTargetPoint = distanceFromTargetPoint;


        //TODO check if the bus approaches to target point
        //TODO check if the bus gets far away from target point
        if (!_loveRunTriggered && _lovesPresident && busIsApproaching &&
            _prevDistanceFromTargetPoint < LoveRunTriggerRange)
        {
            //save previous position
            _posBeforeLoveRun = transform.position;

            //bus is incoming, change state to love run
            _loveRunTriggered = true;
            _moveState = MoveState.MsLoveRun;
	        _actualSpeed = LoveRunSpeed;

	        Vector3 directionFromTargetToPos = (transform.position - _targetPointOnSpline).normalized;

            //move towards a little backwards from bus target spline pos
	        Vector3 posAroundTarget = _targetPointOnSpline + directionFromTargetToPos*MinDistanceFromBusPath;

	        _nextWanderPos = posAroundTarget + new Vector3(Random.Range(-PositionRelaxCoef, PositionRelaxCoef), 
                0, Random.Range(-PositionRelaxCoef, PositionRelaxCoef));
            _wanderDirection = _nextWanderPos - transform.position;

            //print("run to: " + _nextWanderPos);
        }

        if (_moveState == MoveState.MsIdle)
        {
            if (_nextWanderTime < Time.time)
            {
                //wander around
                _moveState = MoveState.MsWandering;

                //TODO select direction and length, make sure you're not stepping on spline
                _wanderDirection = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)).normalized;
                _nextWanderPos = transform.position + _wanderDirection * Random.Range(MinMovementDistance, MaxMovementDistance);
            }
            
        }
        else if (_moveState == MoveState.MsWandering)
        {
            MoveToPositionOnUpdate();

            if (transform.position == _nextWanderPos)
            {
                SetToIdle();
            }                
        }
        else if (_moveState == MoveState.MsLoveRun)
        {
            MoveToPositionOnUpdate();

            if (transform.position == _nextWanderPos)
            {
                //TODO watch the bus until it goes away

                if (!busIsApproaching)
                {
                    //bus is going away, now check if it's beyond a certain range
                    if (distanceFromTargetPoint > LoveRunTriggerRange || Random.Range(1, 100000) < GiveUpCoef)
                    {
                        //print("distance: " + distanceFromTargetPoint);
                        //move back to original position 
                        _moveState = MoveState.MsWandering;

                        //TODO make sure you're not stepping on spline
                        _nextWanderPos = _posBeforeLoveRun +
                                         new Vector3(Random.Range(-PositionRelaxCoef, PositionRelaxCoef), 
                                         0, Random.Range(-PositionRelaxCoef, PositionRelaxCoef));

                        _wanderDirection = _nextWanderPos - transform.position;
                        _actualSpeed = RegularSpeed;
                    }
                }
            }
        }
    }

    public bool getIsImpressed()
    {
        return _isImpressed;
    }

    public MoveState GetMoveState()
    {
        return _moveState;
    }

    void SetToIdle()
    {
        _moveState = MoveState.MsIdle;
        //when you reach it, set new nextWanderTime
        _nextWanderTime = Time.time + Random.Range(MinIdleDuration, MaxIdleDuration);
        //set new target point
        SetTargetPointOnSpline();
    }

    void MoveToPositionOnUpdate()
    {
        //lerp until you reach a certain point
        float step = _actualSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _nextWanderPos, step);

        float rotateStep = RotationSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, _wanderDirection, rotateStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    void SetTargetPointOnSpline()
    {
        if (_loveRunTriggered || !_lovesPresident)
        {
            return;
        }

        float pointDistanceToPedestrian = float.MaxValue;

        //get closest point on path as target point
        for (float splineIntervalIncrement = 0.0f; splineIntervalIncrement < 1.0f; splineIntervalIncrement += 0.0025f)
        {
            Vector3 currentTargetPoint = iTween.PointOnPath(_busPath, splineIntervalIncrement);
            float distanceToCurrentPoint = Vector3.Distance(currentTargetPoint, transform.position);

            if (distanceToCurrentPoint < pointDistanceToPedestrian)
            {
                pointDistanceToPedestrian = distanceToCurrentPoint;
                _targetPointOnSpline = currentTargetPoint;
                
                UpdatePrevDistanceFromTargetPoint();
            }
        }
    }

    void UpdatePrevDistanceFromTargetPoint()
    {
        float distanceFromTargetPoint = Vector3.Distance(_busMoverGameObject.transform.position, _targetPointOnSpline);
        _prevDistanceFromTargetPoint = distanceFromTargetPoint;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Cone" && !_isImpressed)
        {
            _remainingSecondsToImpress -= Time.deltaTime;
            
           SetBodyColor();

            if (_remainingSecondsToImpress <= 0.0f)
            {
               PedestrianImpressed();
            }
        }
    }

    void PedestrianImpressed()
    {
        _isImpressed = true;

        // Increase score
        CameraScript cameraScript = Camera.main.GetComponent<CameraScript>();
        cameraScript.IncreaseScore();

        //TODO jump jump jump
    }

    void SetBodyColor()
    {
        float remainingPercentage = _remainingSecondsToImpress / SecondsRequiredToImpress;

        Color newMatColor;

        if (remainingPercentage > 0.5f)
        {
            //blue to white
            newMatColor = new Color((1-remainingPercentage)*2, (1-remainingPercentage)*2, 1.0f);
        }
        else
        {
            //white to purple
            newMatColor = new Color(1.0f, remainingPercentage * 2, 1.0f);

        }

        if (remainingPercentage <= 0.0f)
        {
            newMatColor = Color.red;
        }

        _bodyRenderer.material.color = newMatColor;
    }
}
