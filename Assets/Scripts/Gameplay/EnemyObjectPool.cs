using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObjectPool<T> : GameObjectPool<T> where T : BaseEnemy
{
    
    public GameObject Get(T enemy,Vector3 pos, Quaternion lookat = new Quaternion())
    {
        
        if(_objects.Count == 0)
        {
            curMax++;
            return Object.Instantiate(enemy.gameObject,pos,lookat);
        }
        else
        {
            
            _objects.TryTake(out GameObject go);
            go.transform.position = pos;
            go.transform.rotation = lookat;
            return go;
        }
    }

    public override void Return(GameObject item)
    {
        item.SetActive(false);//理论上来讲ondisable就会干完初始化的工作.
        
        _objects.Add(item);
    }

    public void Destory(GameObject item)
    {
        _objects.TryTake(out GameObject go);
    }

    protected override void NewObject()
    {

    }


    private void ShrinkReserveList() //when?
    {

    }
}
