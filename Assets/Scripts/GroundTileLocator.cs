using UnityEngine;
using UnityEngine.Tilemaps; // Needed to work with Tilemaps
using System.Collections.Generic; // Needed for List

public class GroundTileLocator : MonoBehaviour
{
    public Tilemap groundTilemap; // Drag your Ground Tilemap here in Inspector
    public List<Vector3> walkablePositions = new List<Vector3>(); // Stores world positions of tiles

    void Start()
    {
        ScanGroundTiles();
    }

    void ScanGroundTiles()
    {
        // Get the area covered by the tilemap
        BoundsInt bounds = groundTilemap.cellBounds;

        // Go through each cell in that area
        foreach (Vector3Int cellPos in bounds.allPositionsWithin)
        {
            // Check if there's a tile in this position
            if (groundTilemap.HasTile(cellPos))
            {
                // Convert cell position to world position (center of tile)
                Vector3 worldPos = groundTilemap.CellToWorld(cellPos) + groundTilemap.cellSize / 2;

                // Add to our list
                walkablePositions.Add(worldPos);
            }
        }

        Debug.Log("Found " + walkablePositions.Count + " walkable tiles.");
    }
}
