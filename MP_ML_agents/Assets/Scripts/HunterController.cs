using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.Serialization;

public class HunterController : Agent
{
    [SerializeField] private Transform target;
    [SerializeField] private PreyController classObject;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private GameObject raySensorObj;
    [SerializeField] private int maximumOnHungerTime;

    private Rigidbody _rigidbody;
    private List<RayPerceptionSensorComponent3D> _raySensors;
    private Vector3 _distanceCheckpoint;
    private Vector3 _lastPreysKnownLocation;

    private int _timeOnHunger;
    private float _timePassed;
    private float _interval = 1f;

    private float _ultimateReward = 50f;
    private float _spottedPreyReward = 15f;
    private float _gettingCloserReward = 0.05f;
    private float _loosingSightOfPreyPenalty = -0.5f;
    private float _fullPenalty = -50f;
    private float _hungerIncreasedPenalty = -1f;

    private float _closestDistanceToTarget;
    private float _lastObservedDistanceToTarget;

    private float _rayAngle;
    private float _maxAngle = 45f;
    private float _angleStep = 5f;

    private bool _enemyAgentSpottedFirstTime;
    private bool _enemyAgentSpotted;

    public override void Initialize()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        _distanceCheckpoint = transform.localPosition;
        _raySensors = new List<RayPerceptionSensorComponent3D>();
        _raySensors.AddRange(raySensorObj.GetComponentsInChildren<RayPerceptionSensorComponent3D>());
        _timeOnHunger = 0;
        _enemyAgentSpotted = false;
        _enemyAgentSpottedFirstTime = false;
        _closestDistanceToTarget = 999f; // High value to be considered as 'unknown'
        _lastObservedDistanceToTarget = _closestDistanceToTarget;
        _lastPreysKnownLocation = new Vector3(0, 0, 0);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(_rayAngle);
        sensor.AddObservation(_enemyAgentSpotted);
        sensor.AddObservation(_lastPreysKnownLocation);
        sensor.AddObservation(_lastObservedDistanceToTarget);
        sensor.AddObservation(_timeOnHunger);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveRotate = actions.ContinuousActions[0];
        float moveForward = actions.ContinuousActions[1];

        // Vector3 velocity = new Vector3(moveX, 0f, moveZ);
        // velocity = velocity.normalized * Time.deltaTime * moveSpeed;
        // transform.localPosition += velocity;

        _rigidbody.MovePosition(transform.position + transform.forward * moveForward * moveSpeed * Time.deltaTime);
        transform.Rotate(0f, moveRotate * moveSpeed, 0f, Space.Self);

        ApplyPenalties();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
        // continuousActions[1] = Input.GetKey(KeyCode.Space) ? 1.0f : 0.0f;;
    }

    private void Update()
    {
        _timePassed += Time.deltaTime;
        if (_timePassed >= _interval)
        {
            _timePassed = 0f;
            _timeOnHunger++;
            AddReward(_hungerIncreasedPenalty);
            LostPreyFromSightPenalty();
            CheckHungerLevel();
        }
    }

    private void LostPreyFromSightPenalty()
    {
        if (_enemyAgentSpottedFirstTime)
        {
            AddReward(_loosingSightOfPreyPenalty);
        }
    }

    private void ApplyPenalties()
    {
        ApplyFallPenalty();
        // ApplyDistancePenalty();
        CheckSensors();
    }

    private void ApplyFallPenalty()
    {
        if (!(transform.localPosition.y < 0)) return;
        AddReward(_fullPenalty * 2);
        classObject.EndEpisode();
        EndEpisode();
    }

    private void CheckHungerLevel()
    {
        if (_timeOnHunger >= maximumOnHungerTime)
        {
            AddReward(_fullPenalty);
            classObject.AddReward(_ultimateReward);
            EndEpisode();
            classObject.EndEpisode();
        }
    }

    private void CheckSensors()
    {
        if (_raySensors.Count == 0) return;

        foreach (var raySensor in _raySensors)
        {
            var rayOutputs = RayPerceptionSensor.Perceive(raySensor.GetRayPerceptionInput()).RayOutputs;
            StartCoroutine(CheckRayView(rayOutputs));
        }
    }

    /**
     * Reward for spotting the target
     */
    private IEnumerator CheckRayView(RayPerceptionOutput.RayOutput[] rayOutputs)
    {
        foreach (var rayOutput in rayOutputs)
        {
            GameObject hit = rayOutput.HitGameObject;
            if (hit == null || !hit.CompareTag(target.tag)) continue;
            _lastPreysKnownLocation = classObject.transform.localPosition;
            var distance = Vector3.Distance(transform.localPosition, hit.transform.localPosition);
            if (distance < _closestDistanceToTarget)
            {
                AddReward(_gettingCloserReward);
                _closestDistanceToTarget = distance;
            }

            _lastObservedDistanceToTarget = distance;
            _enemyAgentSpotted = true;
            if (_enemyAgentSpottedFirstTime) yield break;
            _enemyAgentSpottedFirstTime = true;
            AddReward(_spottedPreyReward);
            yield break;
        }

        _enemyAgentSpotted = false;
    }

    private bool AgentMovedMoreThan(float distanceInMeters)
    {
        var distanceToCheckpoint = transform.position - _distanceCheckpoint;
        if (!(distanceToCheckpoint.magnitude > distanceInMeters)) return false;
        _distanceCheckpoint = transform.position;
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(target.tag))
        {
            AddReward(_ultimateReward);
            classObject.AddReward(_fullPenalty);
            classObject.ChangeGroundColor(Color.green);
            EndEpisode();
            classObject.EndEpisode();
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            AddReward(_fullPenalty * 2);
            classObject.ChangeGroundColor(Color.red);
            EndEpisode();
            classObject.EndEpisode();
        }
    }
}