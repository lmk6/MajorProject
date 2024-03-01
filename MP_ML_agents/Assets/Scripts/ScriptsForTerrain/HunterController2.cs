using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEngine;

namespace ScriptsForTerrain
{
    public class HunterController2 : Agent
    {
        [SerializeField] private Transform target;
        [SerializeField] private PreyController2 classObject;
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private Terrain terrain;
        [SerializeField] private float maximumHeight = 3f;
        [SerializeField] private GameObject raySensor = null;

        private Rigidbody rb;

        private float _ultimateReward = 50f;
        private float _smallReward = 0.3f;
        private float _fullPenalty = -50f;
        private float _smallPenalty = -0.5f;
        private float _stepPenalty = -0.05f;

        private float _rayAngle = 0f;
        private float _maxAngle = 45f;
        private float _angleStep = 5f;

        public override void Initialize()
        {

            rb = GetComponent<Rigidbody>();
        }

        public override void OnEpisodeBegin()
        {
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
            sensor.AddObservation(_rayAngle);
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            float moveRotate = actions.ContinuousActions[0];
            float moveForward = actions.ContinuousActions[1];
            float angleChoice = actions.ContinuousActions[2];
            
            Debug.Log(angleChoice);

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

        private void AdjustRaySensorAngle(float angleChoice)
        {
            if (raySensor == null || angleChoice == 0) return; 
            _rayAngle = angleChoice > 0 ? 
                Mathf.Min(_rayAngle + _angleStep, _maxAngle) : 
                Mathf.Max(_rayAngle - _angleStep, -_maxAngle);

            var currentRotation = raySensor.transform.localRotation.eulerAngles;
            raySensor.transform.localRotation = Quaternion.Euler(_rayAngle, currentRotation.y, currentRotation.z);
        }

        private void ApplyPenalties()
        {
            ApplyFallPenalty();
            ApplyDistancePenalty();
        }

        private void ApplyDistancePenalty()
        {
            AddReward(_stepPenalty);
        }

        private void ApplyFallPenalty()
        {
            if (!(transform.localPosition.y < 0)) return;
            AddReward(_fullPenalty * 2);
            EndEpisode();
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(target.tag))
            {
                AddReward(_ultimateReward);
                classObject.AddReward(_fullPenalty);
                ChangeGroundColor(Color.green);
                EndEpisode();
                classObject.EndEpisode();
            }

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
            // terrain.GetComponent<Renderer>().material.color = color;
        }
    }
}