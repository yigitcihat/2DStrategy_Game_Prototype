using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilDerrick : Build
{
    protected override void Start()
    {
        base.Start();
        splash = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        buildingName = "OIL DERRICK";
    }
    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        //spawn.gameObject.SetActive(false);
        ItemSelection.HandleItemHover(ItemDefinition);
    }
}
