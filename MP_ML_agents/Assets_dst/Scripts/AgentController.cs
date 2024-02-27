using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Random = UnityEngine.Random;

public class AgentController : Agent
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private Transform ground;
    private Color _defaultColor;
    private float _currentDistance = -1;

    private Rigidbody rb;

    private float _width;
    private float _length;

    public override void Initialize()
    {
        _length = ground.GetComponent<Renderer>().bounds.size.x - 1;
        _width = ground.GetComponent<Renderer>().bounds.size.z - 1;

        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(
            Random.Range(-_length / 2, _length / 2),
            0.3f,
            Random.Range(-_width / 2f, _width / 2f)
        );

        target.localPosition = new Vector3(
            Random.Range(-_length / 2, _length / 2),
            0.3f,
            Random.Range(-_width / 2f, _width / 2f)
        );
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(target.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveRotate = actions.ContinuousActions[0];
        float moveForward = actions.ContinuousActions[1];

        // Vector3 velocity = new Vector3(moveX, 0f, moveZ);
        // velocity = velocity.normalized * Time.deltaTime * moveSpeed;
        // transform.localPosition += velocity;
        
        rb.MovePosition(transform.position + transform.forward * moveForward * moveSpeed * Time.deltaTime);
        transform.Rotate(0f, moveRotate * moveSpeed, 0f, Space.Self);
        
        ApplyFallPenalty();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
        // continuousActions[1] = Input.GetKey(KeyCode.Space) ? 1.0f : 0.0f;;
    }

    private void ApplyDistanceReward()
    {
        var distance = Vector3.Distance(transform.localPosition, target.localPosition);
        if (_currentDistance < 0) _currentDistance = distance;
        else if (distance < _currentDistance) AddReward(0.3f);
        else
        {
            AddReward(-0.5f);
        }

        _currentDistance = distance;

        if (transform.localPosition.y < 0)
        {
            AddReward(-100f);
            EndEpisode();
        }
    }

    private void ApplyFallPenalty()
    {
        if (!(transform.localPosition.y < 0)) return;
        AddReward(-100f);
        EndEpisode();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pellet"))
        {
            AddReward(200f);
            ChangeGroundColor(Color.green);
            EndEpisode();
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            AddReward(-100f);
            ChangeGroundColor(Color.red);
            EndEpisode();
        }
    }

    private void ChangeGroundColor(Color color)
    {
        ground.GetComponent<Renderer>().material.color = color;
    }
}