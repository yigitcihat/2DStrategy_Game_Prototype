using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrackConstruction : BuildingConstruction
{
    public GameObject Barracks;
    // Use this for initialization
    public override void Start()
    {
        constructionSize = new Vector2(4, 4);
        base.Start();
        building = Barracks;
    }
}
