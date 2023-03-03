using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrack : Build
{
    private Vector2 _spawnAreaLimits;
    protected override void Start()
    {
        base.Start();
        splash = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        _spawnAreaLimits.x = 6;
        _spawnAreaLimits.y = 6;

        buildingName = "BARRACKS";
    }

    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        ItemSelection.HandleItemHover(ItemDefinition, gameObject);
        // button active
        //spawn.gameObject.SetActive(true);
        //spawn.GetComponent<SpawnButton>().spawnerBarracks = transform.gameObject;
    }
    // get the unit spawn location
    // location is determined to be around the building at the first available tile
    // if all the tiles are occupied, it returns empty
    // if all the tiles empty it upgrades the first unit found by one

    // check tile if on grid
    
    // check tile if on grid
    
}
