using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugOnly : Singleton<DebugOnly>
{
    private GameAssetsManager gameSceneManager;
    // Start is called before the first frame update
    void Start()
    {
        gameSceneManager = GameAssetsManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
