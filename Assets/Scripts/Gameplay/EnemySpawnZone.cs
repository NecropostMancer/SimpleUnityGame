using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnZone : SpawnZone
{
    protected override void GenEntity()
    {
        AddToList(products[Random.Range(0, products.Count)].GetComponent<BaseEnemy>().Clone(Random3DPoint()));
    }

    protected override void GenEntity(int i)
    {
        AddToList(products[i%products.Count].GetComponent<BaseEnemy>().Clone(Random3DPoint()));
    }
}
