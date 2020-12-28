using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    // Start is called before the first frame update

    //private List<BaseEnemy> enemyRef = new List<BaseEnemy>();
    //private List<SpawnZone> spawnRef = new List<SpawnZone>();

    private Dictionary<int, BaseEnemy> enemyRef = new Dictionary<int, BaseEnemy>();
    private Dictionary<int, SpawnZone> spawnRef = new Dictionary<int, SpawnZone>();

    private GameUIManager gameUIManager;
    //private AssetsLoadSystem Loader;

    private int sinceLastCheck = 0;
    private bool Busy = false;

    private bool win = false;

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

    }

    void Fail()
    {
       
    }

    void Start()
    {
        gameUIManager = GameUIManager.instance;
        gameUIManager.InstallUI();
    }

    public void SendUICommand(UICommand cmd)
    {
        gameUIManager.inform(cmd);
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
