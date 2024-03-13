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

    private Rigidbody _rigidbody;
    private RayPerceptionSensorComponent3D _raySensor;
    private Vector3 _distanceCheckpoint;

    private float _ultimateReward = 50f;
    private float _smallReward = 0.55f;
    private float _fullPenalty = -50f;
    private float _smallPenalty = -0.5f;
    private float _stepPenalty = -0.05f;

    private float _distanceConsideredMovement = 1f;
    private float _distanceToTarget;

    private float _rayAngle;
    private float _maxAngle = 45f;
    private float _angleStep = 5f;

    private bool _enemyAgentSpotted;

    public override void Initialize()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        var spawnController = FindObjectOfType<SpawnController>();
        transform.localPosition = spawnController.GetSpawnPoint();
        _distanceCheckpoint = transform.localPosition;
        _raySensor = raySensorObj.GetComponent<RayPerceptionSensorComponent3D>();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(_rayAngle);
        sensor.AddObservation(_enemyAgentSpotted);
        sensor.AddObservation(_distanceToTarget);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveRotate = actions.ContinuousActions[0];
        float moveForward = actions.ContinuousActions[1];
        float angleChoice = actions.ContinuousActions[2];

        // Vector3 velocity = new Vector3(moveX, 0f, moveZ);
        // velocity = velocity.normalized * Time.deltaTime * moveSpeed;
        // transform.localPosition += velocity;

        _rigidbody.MovePosition(transform.position + transform.forward * moveForward * moveSpeed * Time.deltaTime);
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

    private void ApplyDistancePenalty()
    {
        if (!AgentMovedMoreThan(_distanceConsideredMovement))
            AddReward(_smallPenalty);
    }

    private void ApplyFallPenalty()
    {
        if (!(transform.localPosition.y < 0)) return;
        AddReward(_fullPenalty * 2);
        classObject.EndEpisode();
        EndEpisode();
    }

    /**
     * Reward for spotting the target
     */
    private void CheckRayView()
    {
        if (_raySensor == null) return;

        var rayOutputs = RayPerceptionSensor.Perceive(_raySensor.GetRayPerceptionInput())
            .RayOutputs;

        foreach (var rayOutput in rayOutputs)
        {
            GameObject hit = rayOutput.HitGameObject;
            if (hit == null || !hit.CompareTag(target.tag)) continue;
            AddReward(_smallReward);
            var distance = Vector3.Distance(transform.localPosition, hit.transform.localPosition);
            if (distance < _distanceToTarget)
                AddReward(_smallReward);
            _distanceToTarget = distance;
            _enemyAgentSpotted = true;
            break;
        }

        if (_enemyAgentSpotted) return;
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