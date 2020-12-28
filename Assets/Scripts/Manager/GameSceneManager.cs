using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameSceneManager : Singleton<GameSceneManager>
{
    private float loadingProgress;
    private string currentSceneName;
    private bool isWorking;
    
    private AsyncOperation opRef;

    public void LoadBattleScene(int level)
    {
        currentSceneName = "BattleScene" + level.ToString();
        
        AsyncOperation opRef = SceneManager.LoadSceneAsync(currentSceneName); //watch out!
        StartCoroutine(QueryProgress());
    }

    public void LoadMainMenu()
    {
        currentSceneName = "MainMenu";
        AsyncOperation opRef = SceneManager.LoadSceneAsync(currentSceneName); //watch out!
        StartCoroutine(QueryProgress());
    }

    IEnumerator QueryProgress()
    {
        LoadingSceneActive();
        loadingProgress = 0f;
        while (!opRef.isDone)
        {
            loadingProgress = opRef.progress;
            yield return null;
        }
        loadingProgress = 1f;
        isWorking = false;
        opRef = null;
        //and load more non-editor assets...
        LoadingSceneDisable();
    }


    private void LoadingSceneActive()
    {
        SceneManager.LoadScene("LoadingScene");
    }

    private void LoadingSceneDisable()
    {
        SceneManager.UnloadSceneAsync("LoadingScene");
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
