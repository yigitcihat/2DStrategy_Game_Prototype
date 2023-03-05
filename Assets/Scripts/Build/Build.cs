using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;

public class Build : MonoBehaviour
{
    public Vector2 index;
    public List<GroundTile> path;
    public Vector2 downLeftTileIndex;
    public ItemDefinition ItemDefinition;
    public ItemSelection ItemSelection;
    public float HealthPoint = 100f;
    protected Sprite splash;
    protected string buildingName;
    private Vector2 spawnAreaLimits = new Vector2(6, 6);
    public List<GameObject> chosenGrids;
    protected virtual void Start()
    {
        ItemSelection = FindObjectOfType<ItemSelection>();
    }
    protected void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // move unit to unit
            GameManager.instance.destinationIndex = Spawn();
            //Destroy(fakeUnit);

            GameManager.instance.HandleMovement();


        }
    }
    public Vector2 Spawn()
    {
        // get a spawn location from the spawner building
        GameObject spawnLocation = GetSpawnLocation();
        if (spawnLocation == null)
        {
            return Vector2.zero;
        }
        // spawn unit
        GameObject fakeUnit = new GameObject("Targetpos");
        fakeUnit.transform.position = spawnLocation.transform.position;
        fakeUnit.AddComponent<Soldier>().CreatorBuild = this;
        fakeUnit.transform.SetParent(spawnLocation.transform);
        fakeUnit.GetComponent<Unit>().index = spawnLocation.GetComponent<GroundTile>().index;
        
        // set the location flags
        spawnLocation.GetComponent<GroundTile>().isOccupied = true;
        spawnLocation.GetComponent<GroundTile>().hasUnit = true;
        return fakeUnit.GetComponent<Unit>().index;
    }
    // set the information menu
    protected virtual void OnMouseDown()
    {
        ItemSelection.HandleItemHover(ItemDefinition, gameObject);
    }
    // color when cursor is over the building
    protected void OnMouseEnter()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
    }
    // reset color when cursor exits
    protected void OnMouseExit()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
    }
    // flash warning
    protected IEnumerator Warning(SpriteRenderer spriteRenderer)
    {
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
        }
    }
    
    public GameObject GetSpawnLocation()
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

        return null;

    }
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
    public IEnumerator GetDamage(float damage)
    {
        HealthPoint -= damage;
        GetComponentInChildren<SpriteRenderer>().color = Color.red;
        transform.DOShakePosition(0.1f, 0.1f, 1, 5);
        yield return new WaitForSeconds(0.15f);
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
        if (HealthPoint <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        foreach (GameObject tile in chosenGrids)
        {
            tile.GetComponent<GroundTile>().isOccupied = false;
        }
    }
}
