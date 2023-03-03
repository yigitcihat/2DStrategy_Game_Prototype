using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Build : MonoBehaviour
{
    public Vector2 downLeftTileIndex;
    public ItemDefinition ItemDefinition;
    public ItemSelection ItemSelection;
    protected Sprite splash;
    protected Image information;
    protected Text nameArea;
    protected string buildingName;
    protected Button spawn;
    private Vector2 spawnAreaLimits = new Vector2(1,1);
    // get the information panel elements
    protected virtual void Start()
    {
        ItemSelection = FindObjectOfType<ItemSelection>();
        //information = UIManager.instance.informationImage;
        //nameArea = UIManager.instance.informationText;
        //spawn = UIManager.instance.spawn;
    }
    // set the information menu
    protected virtual void OnMouseDown()
    {

        //information.gameObject.SetActive(true);
        //information.sprite = splash;
        //nameArea.text = buildingName;
        ItemSelection.HandleItemHover(ItemDefinition, gameObject);
    }
    // color when cursor is over the building
    protected void OnMouseEnter()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
        ItemSelection.HandleItemHover(ItemDefinition, gameObject);
        
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
