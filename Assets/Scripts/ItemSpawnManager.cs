using System.Collections.Generic;  // Lets us use Lists
using UnityEngine;
using UnityEngine.Tilemaps;        // Needed to work with Tilemaps

public class ItemSpawnManager : MonoBehaviour
{
    public Tilemap spawnTilemap;         // The Tilemap layer where we marked item spawn locations
    public GameObject[] valuablePrefabs; // List of item prefabs to spawn (set in Inspector)

    private List<Vector3> spawnPositions = new List<Vector3>(); // Stores all possible spawn locations

    void Start()
    {
        // Step 1: Get all the spawn positions from the tilemap
        GetSpawnPositionsFromTilemap();

        // Step 2: Spawn items at random positions
        SpawnValuablesRandomly(8); // Number Of Items Spawnable
    }

    // This method finds all tiles in the spawn tilemap and saves their positions
    void GetSpawnPositionsFromTilemap()
    {
        spawnPositions.Clear(); // Make sure the list is empty before adding new positions

        // The area that contains all the tiles in the tilemap
        BoundsInt bounds = spawnTilemap.cellBounds;

        // Loop through every cell in the tilemap's area
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            // Check if there is a tile at this cell position
            if (spawnTilemap.HasTile(pos))
            {
                // Convert the tile's grid position to a world position (so we can place items there)
                Vector3 worldPos = spawnTilemap.CellToWorld(pos) + spawnTilemap.tileAnchor;

                // Add this position to our list of spawn points
                spawnPositions.Add(worldPos);
            }
        }
    }

    // This method spawns a certain number of random items at random spawn positions
    void SpawnValuablesRandomly(int amount)
    {
        // Repeat this for however many items we want to spawn
        for (int i = 0; i < amount; i++)
        {
            // If no spawn points or no prefabs exist, stop the function
            if (spawnPositions.Count == 0 || valuablePrefabs.Length == 0)
                return;

            // Pick a random spawn position from the list
            Vector3 spawnPos = spawnPositions[Random.Range(0, spawnPositions.Count)];

            // Pick a random valuable prefab from the list
            GameObject prefabToSpawn = valuablePrefabs[Random.Range(0, valuablePrefabs.Length)];

            // Create the item in the scene at the chosen position
            Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        }
    }
}
