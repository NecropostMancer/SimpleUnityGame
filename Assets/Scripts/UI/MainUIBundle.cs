using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIBundle : UIBundle
{

    
    private void OnLevelWasLoaded(int level)
    {
        
    }

    void LoadScene()
    {
        GameAssetsManager.instance.LoadSceneByName("LevelSelection");
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }

    // 依赖于prefab的层级结构。
    // Start is called before the first frame update
    void Start()
    {
        Transform y = transform.GetChild(0).GetChild(0);
        y.Find("LOAD").GetComponent<Button>().onClick.AddListener(LoadScene);
        y.Find("NEW").GetComponent<Button>().onClick.AddListener(LoadScene);
        y.Find("QUIT").GetComponent<Button>().onClick.AddListener(QuitGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void SendCommand(UICommand command)
    {
        throw new System.NotImplementedException();
    }
}
