using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameUIManager : Singleton<GameUIManager>
{
    WeakReference<UIBundle> bundle;
    
    public void inform(UICommand cmd)
    {
        UIBundle b;
        bundle.TryGetTarget(out b);
        if (b)
        {
            b.SendCommand(cmd);
        }
    }

    public void InstallUI()
    {
        bundle = new WeakReference<UIBundle>(FindObjectOfType<UIBundle>());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
