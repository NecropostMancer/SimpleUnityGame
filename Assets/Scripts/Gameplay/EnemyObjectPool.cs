using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObjectPool<T> : GameObjectPool where T : BaseEnemy, new()
{
    private readonly BaseEnemy enemy = new T();
    public override GameObject Get()
    {
        if(_objects.Count == 0)
        {
            curMax++;
            return MonoBehaviour.Instantiate(enemy).gameObject;
        }
        else
        {
            
            _objects.TryTake(out GameObject go);
            return go;
        }
    }

    public override void Return(GameObject item)
    {
        item.SetActive(false);
        Animator ani = item.GetComponent<Animator>();
        ani.Play("Wary Rifle", -1);
        ani.Play("Default", 1);
        item.GetComponent<BaseEnemy>().ResetAll();
        _objects.Add(item);
    }

    protected override void NewObject()
    {

    }


    private void ShrinkReserveList() //when?
    {

    }
}
