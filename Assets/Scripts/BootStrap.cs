using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//第一个场景。
public class BootStrap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //确保所有的start和awake都执行好，初始化整个游戏。
    int a = 0;
    // Update is called once per frame
    void Update()
    {
        if (a == 0)
        {
            GameUIManager.instance.InvokeSceneManager(0);
            SceneManager.UnloadSceneAsync("Bootstrap");
            Destroy(this);
        }
        a++;
    }
}
