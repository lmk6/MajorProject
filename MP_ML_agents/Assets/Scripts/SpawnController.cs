using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private float spawnEdgeDistance = 5f;
    [SerializeField] private float minimumDistanceBetweenAgents = 10f;
    [SerializeField] private float maximumHeight = 3f;
    [SerializeField] private Terrain terrain = null;
    [SerializeField] private Transform ground = null;
    [SerializeField] private GameObject obstacle1 = null;
    [SerializeField] private GameObject obstacle2 = null;
    [SerializeField] private GameObject obstacle3 = null;
    [SerializeField] private bool spawnObstacles = true;

    private List<Vector3> _startingSpawnArea;
    private List<Vector3> _validSpawnArea;
    private List<GameObject> _obstacles;
    private readonly float _spawnHeightOffset = 0.3f;

    public Vector3[] GetAgentsSpawnPoints()
    {
        _validSpawnArea = new List<Vector3>(_startingSpawnArea);
        _obstacles.ForEach(Destroy);
        var spawnPoint1 = GetSpawnPoint();
        var spawnPoint2 = GetSpawnPoint();
        if (spawnObstacles)
            SpawnObstacles();
        return new[] { spawnPoint1, spawnPoint2 };
    }

    private void SpawnObstacles()
    {
        var prefabs = new List<GameObject>() { obstacle1, obstacle2, obstacle3 };
        
        prefabs.ForEach(p =>
        {
            if (p != null)
            {
                var spawnPoint = transform.TransformPoint(GetSpawnPoint());
                spawnPoint -= (new Vector3(0, 0.3f, 0));
                _obstacles.Add(Instantiate(p, spawnPoint, GetRotation()));
            }
        });
    }

    private Quaternion GetRotation()
    {
        float randomYRotation = Random.Range(0f, 360f); // Random rotation around Y axis

        return Quaternion.Euler(0f, randomYRotation, 0f);
    }

    private Vector3 GetSpawnPoint()
    {
        if (!_validSpawnArea.Any())
        {
            Debug.Log("NO SPACE TO SPAWN");
            return new Vector3(0, 0.3f, 0);
        }
        var newSpawnPoint = _validSpawnArea[Random.Range(0, _validSpawnArea.Count)];
        RemoveSpawnAreaCloseTo(newSpawnPoint);
        return newSpawnPoint;
    }

    private void RemoveSpawnAreaCloseTo(Vector3 newSpawnPoint)
    {
        _validSpawnArea.RemoveAll(point => 
            Vector3.Distance(newSpawnPoint, point) < minimumDistanceBetweenAgents);
    }

    private void Awake()
    {
        _startingSpawnArea = new List<Vector3>();
        _validSpawnArea = new List<Vector3>();
        _obstacles = new List<GameObject>();

        ComputeStartingSpawnArea();
    }

    private void ComputeStartingSpawnArea()
    {
        if (terrain != null) ComputeValidSpawnAreaForTerrain();
        else if (ground != null) ComputeValidSpawnAreaForGround();
        else Debug.LogError("No Area To Spawn in!!!");
    }

    private void ComputeValidSpawnAreaForGround()
    {
        Vector3 terrainSize = ground.GetComponent<Renderer>().bounds.size;
        float minX = -terrainSize.x / 2 + spawnEdgeDistance;
        float maxX = terrainSize.x / 2 - spawnEdgeDistance;
        float minZ = -terrainSize.z / 2 + spawnEdgeDistance;
        float maxZ = terrainSize.z / 2 - spawnEdgeDistance;

        for (float x = minX; x < maxX; x++)
        {
            for (float z = minZ; z < maxZ; z++)
            {
                float y = _spawnHeightOffset;
                _startingSpawnArea.Add(new Vector3(x, y, z));
            }
        }
    }

    private void ComputeValidSpawnAreaForTerrain()
    {
        Vector3 terrainSize = terrain.terrainData.size;
        float minX = spawnEdgeDistance;
        float maxX = terrainSize.x - spawnEdgeDistance;
        float minZ = spawnEdgeDistance;
        float maxZ = terrainSize.z - spawnEdgeDistance;

        for (float x = minX; x < maxX; x++)
        {
            for (float z = minZ; z < maxZ; z++)
            {
                var worldPos = terrain.transform.TransformPoint(new Vector3(x, 0f, z));
                float y = terrain.SampleHeight(worldPos);

                // Check if the height is within the valid range
                if (y <= maximumHeight)
                {
                    // Adjust y position to lift the agent above terrain
                    y += _spawnHeightOffset;
                    _startingSpawnArea.Add(new Vector3(x, y, z));
                }
            }
        }
    }
}