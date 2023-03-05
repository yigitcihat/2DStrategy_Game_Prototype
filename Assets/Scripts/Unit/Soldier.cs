using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Unit
{
    protected override void Start()
    {
        base.Start();
        if (transform.childCount>0)
        {
            if (transform.GetChild(0).TryGetComponent<SpriteRenderer>(out SpriteRenderer renderer))
            {
                splash = renderer.sprite;
            }
        }
        
        unitName = "SOLDIER";
    }
}
