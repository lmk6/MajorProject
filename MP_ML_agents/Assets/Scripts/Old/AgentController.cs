// using Unity.MLAgents;
// using Unity.MLAgents.Actuators;
// using Unity.MLAgents.Sensors;
// using UnityEngine;
// using Random = UnityEngine.Random;
//
// public class AgentController : Agent
// {
//     [SerializeField] private Transform target;
//     [SerializeField] private float moveSpeed = 4f;
//     [SerializeField] private Transform ground;
//
//     private Color _defaultColor;
//
//     // private float _currentDistance = -1;
//     private float _ultimateReward = 50f;
//     private float _smallReward = 0.3f;
//     private float _fullPenalty = -50f;
//     private float _smallPenalty = -0.5f;
//     private float _stepPenalty = -0.05f;
//
//     private Rigidbody rb;
//     [SerializeField] private GameObject ray;
//     private RayPerceptionSensorComponent3D _raySensor;
//
//     private float _width;
//     private float _length;
//
//     public override void Initialize()
//     {
//         _length = ground.GetComponent<Renderer>().bounds.size.x - 5;
//         _width = ground.GetComponent<Renderer>().bounds.size.z - 5;
//
//         rb = GetComponent<Rigidbody>();
//         if (ray != null)
//             _raySensor = ray.GetComponent<RayPerceptionSensorComponent3D>();
//     }
//
//     public override void OnEpisodeBegin()
//     {
//         transform.localPosition = new Vector3(
//             Random.Range(-_length / 2, _length / 2),
//             1f,
//             Random.Range(-_width / 2f, _width / 2f)
//         );
//
//         target.localPosition = new Vector3(
//             Random.Range(-_length / 2, _length / 2),
//             1f,
//             Random.Range(-_width / 2f, _width / 2f)
//         );
//     }
//
//     public override void CollectObservations(VectorSensor sensor)
//     {
//         sensor.AddObservation(transform.localPosition);
//         // sensor.AddObservation(target.localPosition);
//     }
//
//     public override void OnActionReceived(ActionBuffers actions)
//     {
//         float moveRotate = actions.ContinuousActions[0];
//         float moveForward = actions.ContinuousActions[1];
//
//         // Vector3 velocity = new Vector3(moveX, 0f, moveZ);
//         // velocity = velocity.normalized * Time.deltaTime * moveSpeed;
//         // transform.localPosition += velocity;
//
//         rb.MovePosition(transform.position + transform.forward * moveForward * moveSpeed * Time.deltaTime);
//         transform.Rotate(0f, moveRotate * moveSpeed, 0f, Space.Self);
//
//         ApplyPenalties();
//     }
//
//     public override void Heuristic(in ActionBuffers actionsOut)
//     {
//         ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
//         continuousActions[0] = Input.GetAxisRaw("Horizontal");
//         continuousActions[1] = Input.GetAxisRaw("Vertical");
//         // continuousActions[1] = Input.GetKey(KeyCode.Space) ? 1.0f : 0.0f;;
//     }
//
//     private void ApplyPenalties()
//     {
//         ApplyFallPenalty();
//         ApplyDistancePenalty();
//         ApplyInViewReward();
//     }
//
//     private void ApplyDistancePenalty()
//     {
//         AddReward(_stepPenalty);
//     }
//
//     private void ApplyFallPenalty()
//     {
//         if (!(transform.localPosition.y < 0)) return;
//         AddReward(_fullPenalty * 2);
//         EndEpisode();
//     }
//
//     private void ApplyInViewReward()
//     {
//         if (_raySensor == null) return;
//         
//         var rayOutputs = RayPerceptionSensor.Perceive(_raySensor.GetRayPerceptionInput())
//             .RayOutputs;
//
//         var targetSpotted = false;
//
//         foreach (var rayOutput in rayOutputs)
//         {
//             GameObject hit = rayOutput.HitGameObject;
//             if (hit == null || !hit.CompareTag(target.tag)) continue;
//             // Check if the hit GameObject has the desired tag
//             AddReward(_smallReward);
//             targetSpotted = true;
//         }
//
//         if (targetSpotted) return;
//         AddReward(_smallPenalty);
//     }
//
//     private void OnTriggerEnter(Collider other)
//     {
//         if (other.gameObject.CompareTag(target.tag))
//         {
//             AddReward(_ultimateReward);
//             ChangeGroundColor(Color.green);
//             EndEpisode();
//         }
//
//         if (other.gameObject.CompareTag("Wall"))
//         {
//             AddReward(_fullPenalty);
//             ChangeGroundColor(Color.red);
//             EndEpisode();
//         }
//     }
//
//     private void ChangeGroundColor(Color color)
//     {
//         ground.GetComponent<Renderer>().material.color = color;
//     }
// }