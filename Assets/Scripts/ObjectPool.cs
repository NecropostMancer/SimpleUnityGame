using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//没东西
public abstract class GameObjectPool
{

    protected List<GameObject> reserve = new List<GameObject>();

    


    public abstract GameObject Instantiate();
    public abstract void Destory();

    private void NewObject()
    {

    }

    private void ShrinkReserveList() //when?
    {
        lock (reserve)
        {

        }
    }
}
