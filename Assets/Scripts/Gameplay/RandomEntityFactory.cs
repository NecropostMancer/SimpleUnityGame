using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//deprecated
[System.Obsolete("using prototype for enemygen instead.")]
public class RandomEntityFactory : BaseEntityFactory
{
    
    public override GameObject GenEntity()
    {
        int len = products.Length;
        return products[Random.Range(0, len - 1)];

    }
}
