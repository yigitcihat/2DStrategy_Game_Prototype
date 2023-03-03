using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPlantConstruction : BuildingConstruction
{
    public GameObject PowerPlant;
    // Use this for initialization
    public override void Start()
    {
        constructionSize = new Vector2(3, 3);
        base.Start();
        building = PowerPlant;
    }
}
