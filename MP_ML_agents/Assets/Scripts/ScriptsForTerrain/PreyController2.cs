using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace ScriptsForTerrain
{
    public class PreyController2 : Agent
    {
        [SerializeField] private HunterController2 classObject;
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private Terrain terrain;
        [SerializeField] private float maximumHeight = 3f;

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
            rb = GetComponent<Rigidbody>();
        }

        public override void OnEpisodeBegin()
        {
            EpisodeTimerNew();
            var newPosition = GetStartingPosition();
            while (newPosition is null)
            {
                newPosition = GetStartingPosition();
            }

            transform.localPosition = newPosition.Value;
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

        private Vector3? GetStartingPosition()
        {
            Vector3 terrainPosition = terrain.transform.position;
            Vector3 terrainSize = terrain.terrainData.size;

            float minX = terrainPosition.x;
            float maxX = terrainPosition.x + terrainSize.x;
            float minZ = terrainPosition.z;
            float maxZ = terrainPosition.z + terrainSize.z;

            float x = Random.Range(minX, maxX);
            float z = Random.Range(minZ, maxZ);
            float y = terrain.SampleHeight(new Vector3(x, 0f, z));

            if (y > maximumHeight) return null;
            return new Vector3(x, y, z);
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
}