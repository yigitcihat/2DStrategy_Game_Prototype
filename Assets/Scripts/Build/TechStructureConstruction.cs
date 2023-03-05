using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechStructureConstruction : BuildingConstruction
{
    // Use this for initialization
    public override void Start()
    {
        constructionSize = new Vector2(4, 3);
        base.Start();
        building = Building;
    }
}
