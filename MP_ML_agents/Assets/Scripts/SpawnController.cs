using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private float spawnEdgeDistance = 5f;
    [SerializeField] private float minimumDistanceBetweenAgents = 10f;
    [SerializeField] private float maximumHeight = 3f;
    [SerializeField] private Terrain terrain;
    [SerializeField] private Transform ground;

    private List<Vector3> _validSpawnArea;
    private readonly float _spawnHeightOffset = 0.3f;

    public Vector3 GetSpawnPoint()
    {
        return _validSpawnArea[Random.Range(0, _validSpawnArea.Count)];
    }

    private void Awake()
    {
        _validSpawnArea = new List<Vector3>();

        ComputeValidSpawnArea();
    }

    private void ComputeValidSpawnArea()
    {
        if (terrain is not null) ComputeValidSpawnAreaForTerrain();
        else if (ground is not null) ComputeValidSpawnAreaForGround();
        else Debug.LogError("No Area To Spawn in!!!");
    }

    private void ComputeValidSpawnAreaForGround()
    {
        // Vector3 terrainPosition = ground.transform.localPosition;
        // Vector3 terrainSize = ground.GetComponent<Renderer>().bounds.size;
        // float minX = terrainPosition.x + spawnEdgeDistance;
        // float maxX = terrainPosition.x + terrainSize.x - spawnEdgeDistance;
        // float minZ = terrainPosition.z + spawnEdgeDistance;
        // float maxZ = terrainPosition.z + terrainSize.z - spawnEdgeDistance;

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
                _validSpawnArea.Add(new Vector3(x, y, z));
            }
        }
    }

    private void ComputeValidSpawnAreaForTerrain()
    {
        Vector3 terrainPosition = terrain.transform.localPosition;
        Vector3 terrainSize = terrain.terrainData.size;
        float minX = terrainPosition.x + spawnEdgeDistance;
        float maxX = terrainPosition.x + terrainSize.x - spawnEdgeDistance;
        float minZ = terrainPosition.z + spawnEdgeDistance;
        float maxZ = terrainPosition.z + terrainSize.z - spawnEdgeDistance;

        for (float x = minX; x < maxX; x++)
        {
            for (float z = minZ; z < maxZ; z++)
            {
                float y = terrain.SampleHeight(new Vector3(x, 0f, z));

                // Check if the height is within the valid range
                if (y <= maximumHeight)
                {
                    // Adjust y position to lift the agent above terrain
                    y += _spawnHeightOffset;
                    _validSpawnArea.Add(new Vector3(x, y, z));
                }
            }
        }
    }
}