using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Random = UnityEngine.Random;

public class PreyController : Agent
{
    [SerializeField] private HunterController classObject;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private Transform ground;

    [SerializeField] private int timeForEpisode;
    private float _timeLeft;

    private Color _defaultColor;

    // private float _currentDistance = -1;
    private float _ultimateReward = 50f;
    private float _smallReward = 0.3f;
    private float _fullPenalty = -50f;
    private float _smallPenalty = -0.5f;
    private float _stepPenalty = -0.05f;

    private Rigidbody rb;

    private float _width;
    private float _length;

    public override void Initialize()
    {
        _length = ground.GetComponent<Renderer>().bounds.size.x - 5;
        _width = ground.GetComponent<Renderer>().bounds.size.z - 5;
        
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(
            Random.Range(-_length / 2, _length / 2),
            1f,
            Random.Range(-_width / 2f, _width / 2f)
        );
        
        EpisodeTimerNew();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        // sensor.AddObservation(target.localPosition);
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
        CheckRemainingTime();
    }

    private void ApplyPenalties()
    {
        ApplyFallPenalty();
        ApplyDistancePenalty();
    }

    private void ApplyDistancePenalty()
    {
        AddReward(_smallReward);
    }

    private void ApplyFallPenalty()
    {
        if (!(transform.localPosition.y < 0)) return;
        AddReward(_fullPenalty * 2);
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

    private void ChangeGroundColor(Color color)
    {
        ground.GetComponent<Renderer>().material.color = color;
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