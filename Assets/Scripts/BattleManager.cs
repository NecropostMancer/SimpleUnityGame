using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // Start is called before the first frame update

    private List<BaseEnemy> enemyRef = new List<BaseEnemy>();
    private List<SpawnZone> spawnRef = new List<SpawnZone>();

    private GameUIManager gameUIManager;
    private AssetsLoadSystem Loader;

    private int sinceLastCheck = 0;
    private bool Busy = false;
    public int AddUnitReference(BaseEnemy baseEnemy)
    {

    }
    
    public int RemoveUnitReference(BaseEnemy baseEnemy)
    {

    }

    public int RemoveUnitReference(int index)
    {

    }

    public void SendBattleUIEvent(int type)
    {

    }

    public void SetGamePause(bool state)
    {

    }
    
    IEnumerator StartCheckState()
    {

    }

    void GenNextWave()
    {

    }

    void LoadLevelManifest()
    {

    }



    void Start()
    {
        gameUIManager = GameUIManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        sinceLastCheck++;
        if(sinceLastCheck > 10 && !Busy)
        {
            Busy = true;
        }
    }
}
