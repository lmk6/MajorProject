using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class AgentController : Agent
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private PreyController classObject;
    [SerializeField] private GameObject raySensorObj;

    private Rigidbody _rigidbody;
    private RayPerceptionSensorComponent3D _raySensor;
    
    private float _ultimateReward = 50f;
    private float _smallReward = 0.55f;
    private float _fullPenalty = -50f;
    private float _smallPenalty = -0.5f;
    private float _stepPenalty = -0.05f;

    private float _rayAngle;
    private float _maxAngle = 45f;
    private float _angleStep = 5f;

    private bool _enemyAgentSpotted;
    
    private void CheckRayView()
    {
        if (_raySensor == null) return;

        var rayOutputs = RayPerceptionSensor.Perceive(_raySensor.GetRayPerceptionInput())
            .RayOutputs;

        foreach (var rayOutput in rayOutputs)
        {
            GameObject hit = rayOutput.HitGameObject;
            if (hit == null || !hit.CompareTag(target.tag)) continue;
            AddReward(_smallReward * 10);
            _enemyAgentSpotted = true;
            break;
        }

        if (_enemyAgentSpotted) return;
        _enemyAgentSpotted = false;
    }

    private void AdjustRaySensorAngle(float angleChoice)
    {
        if (raySensorObj == null || angleChoice == 0) return;
        _rayAngle = angleChoice > 0
            ? Mathf.Min(_rayAngle + _angleStep, _maxAngle)
            : Mathf.Max(_rayAngle - _angleStep, -_maxAngle);

        var currentRotation = raySensorObj.transform.localRotation.eulerAngles;
        raySensorObj.transform.localRotation = Quaternion.Euler(_rayAngle, currentRotation.y, currentRotation.z);
    }
}