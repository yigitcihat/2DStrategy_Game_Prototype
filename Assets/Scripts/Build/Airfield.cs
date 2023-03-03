using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airfield : Build
{
    protected override void Start()
    {
        base.Start();
        splash = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        buildingName = "AIRFIELD";
    }
    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        spawn.gameObject.SetActive(false);
    }
}
