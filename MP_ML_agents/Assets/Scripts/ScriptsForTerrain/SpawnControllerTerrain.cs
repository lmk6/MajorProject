using System.Collections.Generic;
using UnityEngine;

namespace ScriptsForTerrain
{
    public class SpawnControllerTerrain : MonoBehaviour
    {
        [SerializeField] private float maximumHeight = 3f;
        [SerializeField] private float spawnEdgeDistance = 20f;
        [SerializeField] private float minimumDistanceBetweenAgents = 30f;
        [SerializeField] private Terrain terrain;

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
    
    
}