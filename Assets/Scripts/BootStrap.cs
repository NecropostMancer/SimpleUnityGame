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
        gameObject.AddComponent<GameAssetsManager>();
        gameObject.AddComponent<GameUIManager>();
        //gameObject.AddComponent<AudioManager>();
        //gameObject.AddComponent<BattleManager>();
        //SceneManager.UnloadSceneAsync("Bootstrap");
        
    }
    float m_Temp;
    private void Update()
    {
        m_Temp += Time.deltaTime;
        if (m_Temp > 3.1)
        {
            AnimationDone();
        }
    }
    void AnimationDone()
    {
        GameAssetsManager.instance.LoadSceneByName("MainMenu");
        Destroy(GetComponent<BootStrap>());
        DontDestroyOnLoad(this.gameObject);
    }
}
