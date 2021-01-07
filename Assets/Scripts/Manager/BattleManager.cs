using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    // Start is called before the first frame update

    //private List<BaseEnemy> enemyRef = new List<BaseEnemy>();
    //private List<SpawnZone> spawnRef = new List<SpawnZone>();

    private readonly Dictionary<int, BaseEnemy> enemyRef = new Dictionary<int, BaseEnemy>();
    private readonly Dictionary<int, SpawnZone> spawnRef = new Dictionary<int, SpawnZone>();

    private GameAssetsManager gameAssetsManager;
    //private AssetsLoadSystem Loader;

    private int sinceLastCheck = 0;
    private bool Busy = false;

    private bool win = false;

    private int score = 0;

    public void AddUnitReference(BaseEnemy baseEnemy)
    {
        enemyRef.Add(baseEnemy.GetInstanceID(), baseEnemy);
    }
    public void AddUnitReference(SpawnZone spawnZone)
    {
        spawnRef.Add(spawnZone.GetInstanceID(), spawnZone);
    }


    public void RemoveUnitReference(BaseEnemy baseEnemy)
    {
        enemyRef.Remove(baseEnemy.GetInstanceID());
        score++;
        Debug.Log(score);
        if(score > 20)
        {
            Win();
        }
    }

    public void RemoveUnitReference(int index)
    {
        enemyRef.Remove(index);
    }

    

    public void SetGamePause(bool state)
    {

    }
    
    IEnumerator StartCheckState()
    {
        yield return null;
        //do sth!
    }

    void GenNextWave()
    {

    }

    void LoadLevelManifest()
    {

    }

    void Win()
    {
        gameAssetsManager.LoadSceneByName("LevelSelection");
    }

    void Fail()
    {
       
    }

    void Start()
    {
        gameAssetsManager = GameAssetsManager.instance;
        //gameAssetsManager.InstallUI();
    }

    public void SendUICommand(UICommand cmd)
    {
        if (gameAssetsManager == null)
        {
            Debug.LogError("No UIManager can be found.");
        }
        else
        {
            //gameAssetsManager.Inform(cmd);
        }
    }

    // Update is called once per frame
    void Update()
    {
        sinceLastCheck++;
        if(sinceLastCheck > 60 && !Busy)
        {
            Busy = true;
            StartCoroutine(StartCheckState());
        }
    }
}
