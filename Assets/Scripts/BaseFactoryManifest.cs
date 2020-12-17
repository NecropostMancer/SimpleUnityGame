using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BaseFactoryManifest
{
    public int maxProgress;
    
    public int genSpeed;

    public BaseFactoryManifest(int maxProgress = 100 , int genSpeed = 10)
    {
        this.maxProgress = maxProgress;
        this.genSpeed = genSpeed;
    }
}
