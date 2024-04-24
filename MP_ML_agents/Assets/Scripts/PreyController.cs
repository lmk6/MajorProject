using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.Serialization;

public class PreyController : Agent
{
    [SerializeField] private Transform enemyAgent;
    [SerializeField] private HunterController classObject;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private Transform terrain;
    [SerializeField] private GameObject raySensorObj;

    private Color _defaultColor;
    private Vector3 _lastHuntersKnownLocation;

    // private float _currentDistance = -1;
    private float _keepingDistanceReward = 0.05f;
    private float _spottedHunterReward = 15f;
    private float _fullPenalty = -50f;
    private float _smallPenalty = -0.5f;
    private float _stepPenalty = -0.05f;

    private float _closestDistanceToTarget;
    private float _lastObservedDistanceToTarget;

    private float _rayAngle;
    private float _maxAngle = 45f;
    private float _angleStep = 5f;

    private Rigidbody rb;
    private RayPerceptionSensorComponent3D _raySensor;

    private bool _enemyAgentSpottedFirstTime;
    private bool _enemyAgentSpotted;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        // var newPosition = GetStartingPosition();
        // while (newPosition is null)
        // {
        //     newPosition = GetStartingPosition();
        // }
        //
        // transform.localPosition = (Vector3) newPosition;
        var spawnController = GetComponentInParent<SpawnController>();
        var spawnPoints = spawnController.GetAgentsSpawnPoints();
        transform.localPosition = spawnPoints[0];
        classObject.transform.localPosition = spawnPoints[1];
        _closestDistanceToTarget = 999f; // High value to be considered as 'unknown'
        _lastObservedDistanceToTarget = _closestDistanceToTarget;
        _lastHuntersKnownLocation = new Vector3(0, 0, 0);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(_rayAngle);
        sensor.AddObservation(_enemyAgentSpotted);
        sensor.AddObservation(_lastObservedDistanceToTarget);
        sensor.AddObservation(_lastHuntersKnownLocation);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveRotate = actions.ContinuousActions[0];
        float moveForward = actions.ContinuousActions[1];
        float angleChoice = actions.ContinuousActions[2];

        // Vector3 velocity = new Vector3(moveX, 0f, moveZ);
        // velocity = velocity.normalized * Time.deltaTime * moveSpeed;
        // transform.localPosition += velocity;

        rb.MovePosition(transform.position + transform.forward * moveForward * moveSpeed * Time.deltaTime);
        transform.Rotate(0f, moveRotate * moveSpeed, 0f, Space.Self);

        AdjustRaySensorAngle(angleChoice);

        ApplyPenalties();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
        continuousActions[2] = Input.GetKey(KeyCode.Q) ? -1f : (Input.GetKey(KeyCode.E) ? 1f : 0f);
        // continuousActions[1] = Input.GetKey(KeyCode.Space) ? 1.0f : 0.0f;;
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

    private void ApplyPenalties()
    {
        ApplyFallPenalty();
        ApplyDistancePenalty();
        CheckRayView();
    }
    
    private void CheckRayView()
    {
        if (_raySensor == null) return;
        
        var rayOutputs = RayPerceptionSensor.Perceive(_raySensor.GetRayPerceptionInput())
            .RayOutputs;

        foreach (var rayOutput in rayOutputs)
        {
            GameObject hit = rayOutput.HitGameObject;
            if (hit == null || !hit.CompareTag(enemyAgent.tag)) continue;
            _lastHuntersKnownLocation = classObject.transform.localPosition;
            var distance = Vector3.Distance(transform.localPosition, hit.transform.localPosition);
            if (distance >= _closestDistanceToTarget)
            {
                AddReward(_keepingDistanceReward);
                _closestDistanceToTarget = distance;
            }
            _lastObservedDistanceToTarget = distance;
            _enemyAgentSpotted = true;
            if (_enemyAgentSpottedFirstTime) return;
            _enemyAgentSpottedFirstTime = true;
            AddReward(_spottedHunterReward);
            return;
        }
        _enemyAgentSpotted = false;
    }

    private void ApplyDistancePenalty()
    {
        // AddReward(_smallReward);
    }

    private void ApplyFallPenalty()
    {
        if (!(transform.localPosition.y < 0)) return;
        AddReward(_fullPenalty * 2);
        classObject.EndEpisode();
        EndEpisode();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            AddReward(_fullPenalty * 2);
            ChangeGroundColor(Color.red);
            EndEpisode();
            classObject.EndEpisode();
        }
    }

    public void ChangeGroundColor(Color color)
    {
        if (terrain != null)
            terrain.GetComponent<Renderer>().material.color = color;
    }
}