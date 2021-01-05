using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameAssetsManager : Singleton<GameAssetsManager>
{
    private float loadingProgress;
    private string currentSceneName;
    private bool isWorking;

    public delegate void callback();

    private AsyncOperation opRef;


    //正确运行依赖于场景的正确命名。
    //BattleScene[1-9]
    //MainMenu
    //LoadingScene
    public void LoadBattleScene(int level,callback cb = null)
    {

        currentSceneName = "BattleScene" + level.ToString();
        
        opRef = SceneManager.LoadSceneAsync(currentSceneName); //watch out!
        StartCoroutine(QueryProgress(cb));
    }

    public void LoadMainMenu(callback cb = null)
    {
        currentSceneName = "MainMenu";
        opRef = SceneManager.LoadSceneAsync(currentSceneName); //watch out!
        StartCoroutine(QueryProgress(cb));
    }

    public void LoadSelection(callback cb = null)
    {
        currentSceneName = "LevelSelection";
        opRef = SceneManager.LoadSceneAsync(currentSceneName); //watch out!
        StartCoroutine(QueryProgress(cb));
    }

    public void LoadStore(callback cb = null)
    {
        currentSceneName = "Store";
        opRef = SceneManager.LoadSceneAsync(currentSceneName); //watch out!
        StartCoroutine(QueryProgress(cb));
    }
    //加载界面有些动画需要控制，因此给它新派了一个类，
    //本着方便的原则，就干脆让sencemanager管理了。
    //目前所有其它的“正常”加载都是通过UI控制的，因此
    //别的类都在调GameUIManager.InvokeSenceManager(int)，并且
    //一些正常的显示行为需要UI显示安排的配合，因此这两个
    //类之间存在了一些来回互相通知，上面两个公共方法事实
    //上只应提供给GameUIManager。
    //如果把UI初始化的职责移交给这里，这个类就有望单独发挥
    //作用，但是代价是两个Manager耦合更深，目前这里还不需要持有
    //UImanager的引用。
    IEnumerator QueryProgress(callback cb = null)
    {
        LoadingSceneActive();
        LoadingControl loadingControl;
        while (true)
        {
            loadingControl = FindObjectOfType<LoadingControl>();
            if(loadingControl != null)
            {
                break;
            }
            yield return null;
        }
        loadingProgress = 0f;
        while (!opRef.isDone)
        {
            loadingProgress = opRef.progress;
            loadingControl.SetProgress(loadingProgress);
            yield return null;
        }
        loadingProgress = 1f;
        isWorking = false;
        opRef = null;
        //and load more non-editor assets...
        loadingControl.StartFadeOut(this);
        //LoadingSceneDisable();
        cb?.Invoke();
    }


    private void LoadingSceneActive()
    {
        SceneManager.LoadScene("LoadingScene",LoadSceneMode.Additive);
        //怎么“合理”地搞定这些相互引用?

    }

    private void LoadingSceneDisable()
    {
        SceneManager.UnloadSceneAsync("LoadingScene");
    }

    //给loadingScene用的，懒得补回调。
    public void RequestLoadingEnd()
    {
        LoadingSceneDisable();
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
