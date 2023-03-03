using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrack : Build
{
    private Vector2 spawnAreaLimits;
    protected override void Start()
    {
        base.Start();
        splash = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        spawnAreaLimits.x = 6;
        spawnAreaLimits.y = 6;

        buildingName = "BARRACKS";
    }

    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        ItemSelection.HandleItemHover(ItemDefinition);
        // button active
        //spawn.gameObject.SetActive(true);
        //spawn.GetComponent<SpawnButton>().spawnerBarracks = transform.gameObject;
    }
    // get the unit spawn location
    // location is determined to be around the building at the first available tile
    // if all the tiles are occupied, it returns empty
    // if all the tiles empty it upgrades the first unit found by one
    public GameObject getSpawnLocation()
    {
        Vector2 spawnpoint = downLeftTileIndex;
        GameObject[,] tileMap = TileManager.instance.tilemap;
        GameObject firstUnitTile = null;
        bool unitFound = false;
        //first candidate grid
        spawnpoint.x--;
        spawnpoint.y--;
        // check for an empty grid around the barracks
        for (int i = (int)spawnpoint.x; i < (int)spawnpoint.x + spawnAreaLimits.x; i++)
        {
            int j = (int)spawnpoint.y;
            if (isGridAvailable(i, j))
            {
                if (!tileMap[i, j].GetComponent<GroundTile>().isOccupied)
                {
                    return tileMap[i, j];
                }
            }
            if (!unitFound && tileMap[i, j].GetComponent<GroundTile>().hasUnit)
            {
                firstUnitTile = tileMap[i, j];
                unitFound = true;
            }
        }
        spawnpoint.x += 5;
        for (int j = (int)spawnpoint.y; j < (int)spawnpoint.y + spawnAreaLimits.y; j++)
        {
            int i = (int)spawnpoint.x;
            if (isGridAvailable(i, j))
            {
                if (!tileMap[i, j].GetComponent<GroundTile>().isOccupied)
                {
                    return tileMap[i, j];
                }
            }
            if (!unitFound && tileMap[i, j].GetComponent<GroundTile>().hasUnit)
            {
                firstUnitTile = tileMap[i, j];
                unitFound = true;
            }
        }
        spawnpoint.y += 5;
        for (int i = (int)spawnpoint.x; i > (int)spawnpoint.x - spawnAreaLimits.x; i--)
        {
            int j = (int)spawnpoint.y;
            if (isGridAvailable(i, j))
            {
                if (!tileMap[i, j].GetComponent<GroundTile>().isOccupied)
                {
                    return tileMap[i, j];
                }
            }
            if (!unitFound && tileMap[i, j].GetComponent<GroundTile>().hasUnit)
            {
                firstUnitTile = tileMap[i, j];
                unitFound = true;
            }
        }
        spawnpoint.x -= 5;
        for (int j = (int)spawnpoint.y; j > (int)spawnpoint.y - spawnAreaLimits.y; j--)
        {
            int i = (int)spawnpoint.x;
            if (isGridAvailable(i, j))
            {
                if (!tileMap[i, j].GetComponent<GroundTile>().isOccupied)
                {
                    return tileMap[i, j];
                }
            }
            if (!unitFound && tileMap[i, j].GetComponent<GroundTile>().hasUnit)
            {
                firstUnitTile = tileMap[i, j];
                unitFound = true;
            }
        }

        // if all the grids are full level up the first tank found
        if (unitFound)
        {
            Vector2 unitIndex = firstUnitTile.transform.GetComponent<GroundTile>().index;
            tileMap[(int)unitIndex.x, (int)unitIndex.y].transform.GetChild(0).GetComponent<Unit>().LevelUp(1);
        }
        // no possible spawn location
        else
        {
            IEnumerator coroutine = Warning(transform.GetChild(0).GetComponent<SpriteRenderer>());
            StartCoroutine(coroutine);
        }

        return null;

    }
    // check tile if on grid
    private bool isGridAvailable(int i, int j)
    {
        if (i >= TileManager.instance.gridWidth || j >= TileManager.instance.gridHeight)
        {
            return false;
        }
        else if (i < 0 || j < 0)
        {
            return false;
        }
        return true;
    }
}
