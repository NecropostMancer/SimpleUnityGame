using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

//没东西
public abstract class GameObjectPool
{

    protected readonly ConcurrentBag<GameObject> _objects;
    //private readonly Func<T> _objectGenerator;
    protected int curMax;


    virtual public GameObject Get()
    {
        GameObject re;
        _objects.TryTake(out re);
        return re;
    }

    virtual public void Return(GameObject item) => _objects.Add(item);

    virtual protected void NewObject()
    {
        
    }
    

    private void ShrinkReserveList() //when?
    {
        
    }
}
