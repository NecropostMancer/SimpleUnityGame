using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEntityFactory : BaseEntityFactory
{
    
    public override GameObject GenEntity()
    {
        int len = products.Length;
        return products[Random.Range(0, len - 1)];

    }
}
