using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    
    private int m_SinceLastCheck = 0;
    private bool m_Busy = false;

    private bool m_Win = false;

    private int m_Wave = 0;

    private bool pause = false;
    [SerializeField]
    private List<Weapon> m_AllWeapon = new List<Weapon>();
    private List<bool> m_PlayerCanUse = new List<bool>();

    private bool m_Sleeping = true;

    private float m_TimeSinceBattle;
    private int m_DeathCount;

    private int g_Mode;
    private int g_Require;

    private bool m_isSpecialTargetDead = false;

    private bool f_LockCursor;
    private bool m_LockCursor
    {
        set
        {
            if (value)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            f_LockCursor = value;
        }
        get
        {
            return f_LockCursor;
        }
    }

    public class WaveUIMsg : UICommand
    {
        public bool g_EndType;
        public int g_Wave;

        public bool g_IsLast;

        public WaveUIMsg(int wave, bool isWaveEnd = false, bool isLast = false)
        {
            g_Wave = wave;
            g_EndType = isWaveEnd;
            g_IsLast = isLast;
        }
    }

    public class HitMsg : UICommand
    {

    }

    public void SetMode(int mode, int goal)
    {
        g_Mode = mode;
        g_Require = goal;
    }

    void LoadLevelManifest()
    {

    }

    void Win()
    {
        //m_GameAssetsManager.LoadSceneByName("LevelSelection");
        m_Win = true;
        GameAssetsManager.instance.LoadEndBattleScene();
        PauseGame(this);
        EndBattle();
    }

    void Fail()
    {
        //m_GameAssetsManager.LoadSceneByName("LevelSelection");
        m_Win = false;
        GameAssetsManager.instance.LoadEndBattleScene();
        PauseGame(this);
        EndBattle();
    }
    public void EndBattle()
    {
        m_Sleeping = true;
        StopAllCoroutines();
        
    }
    public void QuitBattle()
    {

        m_Sleeping = true;
        noIShouldntBeHere = true;

        m_Busy = false;
        if (pause)
        {
            PauseGame(this);
            
        }
        m_LockCursor = false;
        
    }

    void Start()
    {
        CurUpdate = PreBattleUpdate;
        //gameAssetsManager.InstallUI();
    }

    public void SendUICommand(UICommand cmd)
    {
        if (GameAssetsManager.instance == null)
        {
            Debug.LogError("No AssetsManager can be found.");
        }
        else
        {
            //gameAssetsManager.Inform(cmd);
        }
    }

    //不至于不至于
    ConcurrentQueue<BattleEvents> m_eventQueue = new ConcurrentQueue<BattleEvents>();
    public void SendEvents(BattleEvents events)
    {
        m_eventQueue.Enqueue(events);
    }

    bool noIShouldntBeHere = false;
    // Update is called once per frame
    void Update()
    {
        if (m_Sleeping) return;
        CurUpdate();
    }

    private delegate void FuncUpdate();
    FuncUpdate CurUpdate;
    
    private void PreBattleUpdate()
    {
        m_TimeSinceBattle += Time.deltaTime;
        if(m_TimeSinceBattle > 6)
        {
            CurUpdate = InBattleUpdate;
            CharacterManager.instance.StartRecord();
            m_TimeSinceBattle = 0;
        }

    }

    private void InBattleUpdate()
    {
        m_SinceLastCheck++;
        m_TimeSinceBattle += Time.deltaTime;
        if (m_SinceLastCheck > 60 && !m_Busy)
        {
            m_SinceLastCheck = 0;
            if (noIShouldntBeHere)
            {
                QuitBattle();
                noIShouldntBeHere = false;
            }

            m_Busy = true;
            StartCoroutine(StartCheckState());
        }
    }


    //time consuming here
    IEnumerator StartCheckState()
    {
        switch (g_Mode)
        {
            case 0://waves.
                if (m_Wave > g_Require)
                {
                    Win();
                }
                break;
            case 1://survival.
                if (m_TimeSinceBattle > g_Require)
                {
                    Win();
                }
                break;
            case 2://infinity.
                break;
            case 3://boss.
                if (m_isSpecialTargetDead)
                {
                    Win();
                }
                break;
        }
        m_Busy = false;
        yield return null;
        //do sth!
    }

    public void StartBattle(int mode,int goal)
    {
        SetMode(mode, goal);
        m_Wave = 0;
        m_DeathCount = 0;
        m_TimeSinceBattle = 0;
        m_isSpecialTargetDead = false;
        m_Sleeping = false;
        noIShouldntBeHere = false;
        CharacterManager.instance.ResetHP();
        CurUpdate = PreBattleUpdate;
        m_LockCursor = true;
    }

    public List<Weapon> GetAllWeapon()
    {
        return m_AllWeapon;
    }

    public List<Weapon> GetCurrentWeapon()
    {
        m_PlayerCanUse = new List<bool>(GameAssetsManager.instance.GetSave().unlockedWeapon);
        List<Weapon> weapons = new List<Weapon>();
        for(int i = 0; i < m_AllWeapon.Count; i++)
        {
            if (m_PlayerCanUse[i])
            {
                weapons.Add(m_AllWeapon[i]);
            }
        }
        return weapons;
    }

    public void InformKill()
    {
        m_DeathCount++;
        GameUIManager.instance.Inform(new HitMsg());
        Debug.Log("poor soul.");
    }

    public void InformSpecialKill()
    {
        m_isSpecialTargetDead = true;
        Debug.Log("oof.");
    }

    public void InformPlayerDead()
    {
        if (m_Sleeping)
        {
            return;
        }
        Fail();
    }

    public void InformNextWaveCame()
    {
        Debug.LogWarning("NEW WAVE");
        m_Wave++;
        if(m_Wave <= g_Require && g_Require != 0)
        {
            if (m_Wave == g_Require && g_Mode == 0)
            {
                GameUIManager.instance.Inform(new WaveUIMsg(m_Wave,false,true));
            }
            else
            {
                GameUIManager.instance.Inform(new WaveUIMsg(m_Wave,false));
            }
        }
    }

    public void InformWaveEnd()
    {
        StopAllCoroutines();
        if (m_Wave == g_Require && g_Mode == 0)
        {
            GameUIManager.instance.Inform(new WaveUIMsg(m_Wave, true, true));
            
        }
        else
        {
            GameUIManager.instance.Inform(new WaveUIMsg(m_Wave, true));
        }
        m_SinceLastCheck = -300; //delay for ui popup
    }
    
    public void InfromAllWaveEnd()
    {
        if(g_Mode != 0)
        {
            Debug.LogWarning("Battle: Only Death Match(mode 0) should make this.");
        }
        if(m_Wave != g_Require)
        {
            Debug.LogWarning("Battle: m_Wave != g_Require");
        }
        m_Wave++;
    }

    private MonoBehaviour m_PauseSource;
    
    public void PauseGame(MonoBehaviour source)
    {
        if (m_PauseSource != null && source != m_PauseSource) return;
        m_PauseSource = source;
        if (!pause)
        {
            if (source is PlayerController)
            {
                if(!GameUIManager.instance.CallDialog("QUIT LEVEL?", delegate {
                    PauseGame(source);

                    EndBattle();
                    QuitBattle();
                    GameAssetsManager.instance.LoadSceneByName("LevelSelection");
                }, delegate { PauseGame(source); }))
                {
                    m_PauseSource = null;
                    return;
                }
            }
            Time.timeScale = 0;
            pause = true;
            m_LockCursor = false;
            
        }
        else
        {
            Time.timeScale = 1;
            pause = false;
            m_LockCursor = true;
            m_PauseSource = null;
            
        }
    }
    public float GetTimePassed()
    {
        return m_TimeSinceBattle;
    }
    public bool isWin()
    {
        return m_Win;
    }

    public int GetReq()
    {
        return g_Require;
    }

    public int GetMode()
    {
        return g_Mode;
    }
}
