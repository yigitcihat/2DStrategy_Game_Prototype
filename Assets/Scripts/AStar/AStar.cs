using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a* algorithm with priority queue improvement
public class AStar : MonoBehaviour
{
    public Transform player;
    public Vector2 targetIndex;
    public bool hasTarget;

    GameObject[,] tilemap;

    private void Start()
    {
        tilemap = TileManager.instance.tilemap;
        hasTarget = false;
        player = null;
    }
    private void Update()
    {
        // works when both the source and the destination is set
        if (player != null && hasTarget == true)
        {
            List<GroundTile> path = FindPath(player.GetComponent<Unit>().index, targetIndex);
            if (path != null)
            {
                // set the path of the unit
                player.gameObject.GetComponent<Unit>().MoveOnNewPath(path);
            }
            // reset the variables
            player = null;
            hasTarget = false;
        }
    }
    private List<GroundTile> FindPath(Vector2 startIndex, Vector2 targetIndex)
    {
        // get the source & destination tile
        GroundTile source = GetTileFromIndex(startIndex);
        GroundTile destination = GetTileFromIndex(targetIndex);
        // set the heap
        PriorityQ<GroundTile> openSet = new PriorityQ<GroundTile>(tilemap.GetLength(0) * tilemap.GetLength(1));
        HashSet<GroundTile> closedSet = new HashSet<GroundTile>();

        openSet.Add(source);

        while (openSet.getSize() > 0)
        {
            GroundTile current = openSet.ExtractFirstItem();
            closedSet.Add(current);

            // end
            if (current == destination)
            {
                return GetPath(source, destination);
            }
            // examine each neighbour
            foreach (GroundTile neighbourTile in GetNeighbourTiles(current))
            {
                // tile is occupied
                if (neighbourTile != destination)
                {
                    if (neighbourTile.isOccupied == true || closedSet.Contains(neighbourTile))
                    {
                        continue;
                    }
                }
                // destination tile is occupied by a building
                else
                {
                    if (neighbourTile.hasUnit == false && neighbourTile.isOccupied == true || closedSet.Contains(neighbourTile))
                    {
                        continue;
                    }
                }
                // get the neighbout tile having the least cost
                int movementCost = current.gCost + GetDistance(current, neighbourTile);
                if (movementCost < neighbourTile.gCost || !openSet.IsItemInQ(neighbourTile))
                {
                    GroundTile neighbour = neighbourTile;
                    neighbour.gCost = movementCost;
                    neighbour.hCost = GetDistance(neighbourTile, destination);
                    neighbour.hCost = GetDistance(neighbourTile, destination);
                    neighbour.parentTile = current;

                    if (!openSet.IsItemInQ(neighbourTile))
                    {
                        openSet.Add(neighbourTile);
                    }
                    else
                    {
                        openSet.Update(neighbourTile);
                    }
                }
            }
        }
        return null;
    }
    // retrieve the tile from the grid
    private GroundTile GetTileFromIndex(Vector2 index)
    {
        return tilemap[(int)index.x, (int)index.y].GetComponent<GroundTile>();
    }
    // check grid to retrieve the neighbour tiles
    private List<GroundTile> GetNeighbourTiles(GroundTile currentTile)
    {

        List<GroundTile> neighbours = new List<GroundTile>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                //self
                if (i == 0 && j == 0)
                {
                    continue;
                }
                int xIndex = i + (int)currentTile.index.x;
                int yIndex = j + (int)currentTile.index.y;

                if (xIndex >= 0 && xIndex < tilemap.GetLength(0) && yIndex >= 0 && yIndex < tilemap.GetLength(1))
                {
                    neighbours.Add(tilemap[xIndex, yIndex].GetComponent<GroundTile>());
                }
            }
        }
        return neighbours;
    }
    // compute distance between tiles
    private int GetDistance(GroundTile currentTile, GroundTile nextTile)
    {
        int xDistance = Mathf.Abs((int)currentTile.index.x - (int)nextTile.index.x);
        int yDistance = Mathf.Abs((int)currentTile.index.y - (int)nextTile.index.y);

        if (xDistance > yDistance)
        {
            return 14 * yDistance + 10 * (xDistance - yDistance);
        }
        return 14 * xDistance + 10 * (yDistance - xDistance);
    }
    // return the final path using parent relations
    private List<GroundTile> GetPath(GroundTile sourceTile, GroundTile destinationTile)
    {
        List<GroundTile> path = new List<GroundTile>();
        GroundTile current = destinationTile;

        while (current != sourceTile)
        {
            path.Add(current);
            current = current.parentTile;
        }
        path.Reverse();
        return path;
    }

}
