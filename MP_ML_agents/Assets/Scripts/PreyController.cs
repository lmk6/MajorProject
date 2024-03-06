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
    [SerializeField] private int timeForEpisode;
    
    private float _timeLeft;

    private Color _defaultColor;

    // private float _currentDistance = -1;
    private float _ultimateReward = 50f;
    private float _smallReward = 0.3f;
    private float _fullPenalty = -50f;
    private float _smallPenalty = -0.5f;
    private float _stepPenalty = -0.05f;

    private float _rayAngle;
    private float _maxAngle = 45f;
    private float _angleStep = 5f;

    private Rigidbody rb;
    private RayPerceptionSensorComponent3D _raySensor;

    private bool _enemyAgentSpotted;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        EpisodeTimerNew();
        // var newPosition = GetStartingPosition();
        // while (newPosition is null)
        // {
        //     newPosition = GetStartingPosition();
        // }
        //
        // transform.localPosition = (Vector3) newPosition;
        var spawnController = FindObjectOfType<SpawnController>();
        transform.localPosition = spawnController.GetSpawnPoint();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        // sensor.AddObservation(target.localPosition);
        sensor.AddObservation(_rayAngle);
        sensor.AddObservation(_enemyAgentSpotted);
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

    private void Update()
    {
        CheckRemainingTime();
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
            _enemyAgentSpotted = true;
            break;
        }

        if (_enemyAgentSpotted) return;
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
            AddReward(_fullPenalty);
            ChangeGroundColor(Color.red);
            EndEpisode();
            classObject.EndEpisode();
        }
    }

    public void ChangeGroundColor(Color color)
    {
        terrain.GetComponent<Renderer>().material.color = color;
    }

    private void EpisodeTimerNew()
    {
        _timeLeft = Time.time + timeForEpisode;
    }

    private void CheckRemainingTime()
    {
        if (Time.time >= _timeLeft)
        {
            AddReward(_ultimateReward);
            classObject.AddReward(_fullPenalty);
            EndEpisode();
            classObject.EndEpisode();
        }
    }
}