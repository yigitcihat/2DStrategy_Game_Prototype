using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrack : Build
{
    //private Vector2 _spawnAreaLimits;
    protected override void Start()
    {
        base.Start();
        splash = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        //_spawnAreaLimits.x = 6;
        //_spawnAreaLimits.y = 6;

        buildingName = "BARRACKS";
    }

    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        ItemSelection.HandleItemHover(ItemDefinition, gameObject);
    }
    
}
