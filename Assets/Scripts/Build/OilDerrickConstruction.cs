using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilDerrickConstruction : BuildingConstruction
{
    public GameObject OilDerrick;
    // Use this for initialization
    public override void Start()
    {
        constructionSize = new Vector2(3, 3);
        base.Start();
        building = OilDerrick;
    }
}
