using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingConstruction : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject draggedObject;
    public GameObject Building;
    public ItemDefinition ItemDefinition;
    private ItemSelection _itemSelection;
    protected GameObject building;
    protected Vector2 constructionSize;

    private Vector3 startPos;
    private Transform initialParent;
    private List<SpriteRenderer> coloredTiles;
    private GameObject initialTile;
    private int constructableTileCount;

    public virtual void Start()
    {
        coloredTiles = new List<SpriteRenderer>();
        _itemSelection= FindObjectOfType<ItemSelection>();
    }
    // Drag handlers
    public void OnBeginDrag(PointerEventData eventData)
    {
        draggedObject = gameObject;
        _itemSelection.HandleItemHover(ItemDefinition,gameObject);
        startPos = transform.position;
        initialParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        // raycast to mouse position
        transform.position = Input.mousePosition + new Vector3(10,10,0);
        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //clear the colored tiles from last drag
        foreach (SpriteRenderer spriteRenderer in coloredTiles)
        {
            spriteRenderer.color = Color.white;
        }
        //hit
        if (Physics.Raycast(ray, out hit))
        {
            // if ray hits the grid
            if (hit.transform.gameObject.tag == "Ground")
            {
                // get the downleft tile 
                initialTile = hit.transform.gameObject;
                // get the grid to color
                List<List<GameObject>> chosenGrids = decideGrid(constructionSize, hit.transform.gameObject.GetComponent<GroundTile>().index);
                coloredTiles = new List<SpriteRenderer>();
                // selection grid
                constructableTileCount = chosenGrids[0].Count;

                // no obstacle on construction site
                foreach (GameObject tile in chosenGrids[0])
                {
                    tile.GetComponent<SpriteRenderer>().color = Color.green;
                    coloredTiles.Add(tile.GetComponent<SpriteRenderer>());
                }
                // obstacle on construction site
                foreach (GameObject tile in chosenGrids[1])
                {
                    tile.GetComponent<SpriteRenderer>().color = Color.red;
                    coloredTiles.Add(tile.GetComponent<SpriteRenderer>());
                }
            }
            // ray hits obstacle
            else
            {
                // flash the area
                IEnumerator coroutine = Warning(hit.transform.GetChild(0).GetComponent<SpriteRenderer>());
                StartCoroutine(coroutine);
                constructableTileCount = 0;
                return;
            }

        }
        // no hit
        else
        {
            constructableTileCount = 0;
        }

    }
    public void OnEndDrag(PointerEventData eventData)
    {
        List<GameObject> chosenGrids = getGrid(constructionSize, initialTile.GetComponent<GroundTile>().index);
        // if the selected grid is the same size with the building area
        if (constructableTileCount == constructionSize.x * constructionSize.y)
        {
            foreach (GameObject tile in chosenGrids)
            {
                tile.GetComponent<GroundTile>().isOccupied = true;
                Building.GetComponent<Build>().chosenGrids.Add(tile);
            }
            Building.GetComponent<Build>().chosenGrids.RemoveAll(GameObject => GameObject == null);
            // put bulding on location
            GameObject tmp = Instantiate(building, initialTile.transform.position, initialTile.transform.rotation);
            tmp.GetComponent<Build>().downLeftTileIndex = initialTile.GetComponent<GroundTile>().index;
            
        }
        // not a suitable location
        else if (constructableTileCount > 0)
        {
            foreach (GameObject tile in chosenGrids)
            {
                tile.GetComponent<GroundTile>().StartCoroutine("FlashWarning");
            }
        }
        // reset the chosen grid color
        foreach (SpriteRenderer spriteRenderer in coloredTiles)
        {
            spriteRenderer.color = Color.white;
        }

        // reset the dragged object
        draggedObject = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        if (transform.parent == initialParent)
        {
            transform.position = startPos;

        }

    }
    // get grid to place object
    private List<List<GameObject>> decideGrid(Vector2 size, Vector2 tileLocation)
    {
        List<List<GameObject>> result = new List<List<GameObject>>();
        result.Add(new List<GameObject>());
        result.Add(new List<GameObject>());

        for (int i = (int)tileLocation.x; i < (int)tileLocation.x + size.x; i++)
        {
            for (int j = (int)tileLocation.y; j < (int)tileLocation.y + size.y; j++)
            {
                if (!isGridAvailable(i, j))
                {
                    continue;
                }
                // select
                if (!TileManager.instance.tilemap[i, j].GetComponent<GroundTile>().isOccupied)
                {
                    result[0].Add(TileManager.instance.tilemap[i, j]);
                }
                // reject
                else
                {
                    result[1].Add(TileManager.instance.tilemap[i, j]);
                }
            }
        }
        return result;
    }
    private List<GameObject> getGrid(Vector2 size, Vector2 tileLocation)
    {
        List<GameObject> result = new List<GameObject>();

        for (int i = (int)tileLocation.x; i < tileLocation.x + size.x; i++)
        {
            for (int j = (int)tileLocation.y; j < tileLocation.y + size.y; j++)
            {
                if (!isGridAvailable(i, j))
                {
                    continue;
                }
                result.Add(TileManager.instance.tilemap[i, j]);
            }
        }
        return result;
    }

    // check if tile in the grid
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
    // flash warning
    IEnumerator Warning(SpriteRenderer spriteRenderer)
    {
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
        }
    }
}
