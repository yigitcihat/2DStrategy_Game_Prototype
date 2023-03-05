using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechStructure : Build
{
    protected override void Start()
    {
        base.Start();
        splash = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        buildingName = "TECHSTRUCTURE";
    }
    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        ItemSelection.HandleItemHover(ItemDefinition, gameObject);
    }
}
