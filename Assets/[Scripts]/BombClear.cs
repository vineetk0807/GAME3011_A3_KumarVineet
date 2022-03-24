using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombClear : GemClear
{
    public override void ClearGem()
    {
        base.ClearGem();
        
        gem.gridRef.BombClearFunction(gem.X, gem.Y);

        Debug.Log("Bomb clear");
    }
}
