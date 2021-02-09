using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameAssetsManager : Singleton<GameAssetsManager>
{
    private float m_LoadingProgress;
    private string m_CurrentSceneName = "Bootstrap";
    private string m_LastSceneName;
    private int addParamA, addParamB;
    private bool m_IsWorking;

    public delegate void callback();

    private AsyncOperation m_OpLoadingNewRef;
    private AsyncOperation m_OpUnLoadingOldRef;
    private AsyncOperation m_LoadingTransition;
    private UIBundle m_Bundle;

    private ProfileData m_ProfileData;
    //正确运行依赖于场景的正确命名。
    //BattleScene[1-9]
    //MainMenu
    //LoadingScene
    //LevelSelection
    //Store
    public void LoadSceneByName(string name,callback cb = null,int addtionalParamA = -1, int addtionalParamB = -1)
    {
        
        m_LastSceneName = m_CurrentSceneName;
        m_CurrentSceneName = name;
        
        addParamA = addtionalParamA;
        addParamB = addtionalParamB;
        StartCoroutine(QueryProgress(cb));
    }
    
    public void LoadEndBattleScene()
    {
        if (LevelEnd.gm_CurrentInstance == null)
        {
            SceneManager.LoadSceneAsync("EndLevelAddtive", LoadSceneMode.Additive);
        }
    }
    private bool m_enableFore = true;
    private bool m_enablePost = true;
    public void SetTransitionSetting(bool enableFore,bool enablePost)
    {
        m_enableFore = enableFore;
        m_enablePost = enablePost;
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
    //1/5:
    //UImanager 有啥用？删了算了。
    //1/20:回来了！
    //许多的return null是控制加载和卸载帧的
    //目前的问题是加载新场景后会有一两帧会有两个audiolistener，不知道怎么办。
    IEnumerator QueryProgress(callback cb = null)
    {
        LoadingSceneActive();
        m_LoadingProgress = 0f;
        while (!m_LoadingTransition.isDone)
        {
            yield return null;
        }
        yield return null;
        LoadingControl.sm_currentInstance.SetSetting(m_enableFore,m_enablePost);
        LoadingControl.sm_currentInstance.StartFadeIn();
        while(m_OpLoadingNewRef == null)
        {
            yield return null;
        }
        if(m_OpUnLoadingOldRef != null)
        {
            while (!m_OpUnLoadingOldRef.isDone)
            {
                m_LoadingProgress = m_OpUnLoadingOldRef.progress * 0.3f;
                LoadingControl.sm_currentInstance.SetProgress(m_LoadingProgress);
                yield return null;
            }
        }
        m_OpLoadingNewRef.allowSceneActivation = true;
        yield return null;// 下一帧再载入备用的listener
        LoadingControl.sm_currentInstance.SetAudioListenerActive(true);
        while (!m_OpLoadingNewRef.isDone)
        {
            m_LoadingProgress = 0.3f + m_OpLoadingNewRef.progress * 0.5f;
            LoadingControl.sm_currentInstance.SetProgress(m_LoadingProgress);
            yield return null;
        }

        LoadingControl.sm_currentInstance.SetAudioListenerActive(false);
        
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(m_CurrentSceneName));
        
        
        m_LoadingProgress = 1f;
        LoadingControl.sm_currentInstance.SetProgress(1.1f);
        m_IsWorking = false;
        m_OpLoadingNewRef = null;
        //and load more non-editor assets...
        
        if(m_CurrentSceneName[0] == 'B') { BattleManager.instance.StartBattle(addParamA,addParamB); }
        cb?.Invoke();
    }


    private void LoadingSceneActive()
    {
        m_LoadingTransition = SceneManager.LoadSceneAsync("LoadingScene",LoadSceneMode.Additive);
        

    }

    private bool firstGame = true;

    private void LoadingSceneDisable()
    {
        SceneManager.UnloadSceneAsync("LoadingScene");
        m_LoadingTransition = null;
        if(firstGame && m_CurrentSceneName[0] == 'L')
        {
            GameUIManager.instance.CallMsgbox("Only Level 1 is playable.", null, "Fine.");
            firstGame = false;
        }
    }

    //给loadingScene用的，懒得补回调。
    public void RequestLoadingEnd()
    {
        LoadingSceneDisable();
    }

    public bool IsBusy()
    {
        return m_LoadingTransition != null; 
    }

    public void RequesetLoadingStart()
    {
        if(m_LastSceneName != null)
        {
            m_OpUnLoadingOldRef = SceneManager.UnloadSceneAsync(m_LastSceneName); //ok?
        }
        
        m_OpLoadingNewRef = SceneManager.LoadSceneAsync(m_CurrentSceneName,LoadSceneMode.Additive); //watch out!
        m_OpLoadingNewRef.allowSceneActivation = false;
    }
    
    public bool IsSaveVaild()
    {
        try
        {
            AssetsLoadSystem.LoadSave(0);
        }
        catch
        {
            return false;
        }
        return true;
    }

    public ProfileData LoadSave()
    {
        try
        {
            m_ProfileData = AssetsLoadSystem.LoadSave(0);
        }
        catch
        {

        }
        m_ProfileData.unlockedWeapon[0] = true;
        return m_ProfileData;
    }
    public ProfileData GetSave()
    {
        return m_ProfileData;
    }
    public void UpdateSave()
    {
        AssetsLoadSystem.UpdateSave(0,m_ProfileData);
    }
    public void NewSave()
    {
        m_ProfileData = new ProfileData();
        ProfileData.ResetProfile(m_ProfileData);
    }
    private GameSetting m_GameSettingData;
    public bool IsSettingVaild()
    {
        try
        {
            AssetsLoadSystem.LoadSetting();
        }
        catch
        {
            return false;
        }
        return true;
    }

    public GameSetting LoadSetting()
    {
        try
        {
            m_GameSettingData = AssetsLoadSystem.LoadSetting();
        }
        catch
        {

        }
        
        return m_GameSettingData;
    }
    public GameSetting GetSetting()
    {
        return m_GameSettingData;
    }
    public void UpdateSetting()
    {
        AssetsLoadSystem.UpdateSetting(m_GameSettingData);
    }
    public void NewSetting()
    {
        m_GameSettingData = new GameSetting();
        GameSetting.ResetAll(m_GameSettingData);
    }


    // Start is called before the first frame update
    void Start()
    {
        if (IsSettingVaild())
        {
            LoadSetting();
        }
        else
        {
            NewSetting();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
