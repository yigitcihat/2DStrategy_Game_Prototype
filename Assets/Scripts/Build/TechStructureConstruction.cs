using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechStructureConstruction : BuildingConstruction
{
    public GameObject TechStructure;
    // Use this for initialization
    public override void Start()
    {
        constructionSize = new Vector2(4, 3);
        base.Start();
        building = TechStructure;
    }
}
