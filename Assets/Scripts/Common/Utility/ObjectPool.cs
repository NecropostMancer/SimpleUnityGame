using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

//没东西
public abstract class GameObjectPool<T>
{

    protected ConcurrentBag<GameObject> _objects = new ConcurrentBag<GameObject>();
    //private readonly Func<T> _objectGenerator;
    protected int curMax;


    virtual public GameObject Get(T t)
    {
        _objects.TryTake(out GameObject re);
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
