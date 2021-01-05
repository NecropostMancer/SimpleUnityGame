using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//子类沙盒
public class Playable : MonoBehaviour
{
    public virtual void Awake()
    {
        if (battleManager == null)
        {
            battleManager = BattleManager.instance;
        }
    }


    private static BattleManager battleManager;

    protected void SendUICommand(UICommand cmd) {
        
        battleManager.SendUICommand(cmd);
    }

    protected void AddUnitReference(BaseEnemy baseEnemy)
    {
        battleManager.AddUnitReference(baseEnemy);
    }

    protected void RemoveUnitReference(BaseEnemy baseEnemy)
    {
        battleManager.RemoveUnitReference(baseEnemy);
    }

    protected void AddUnitReference(SpawnZone spawnZone)
    {
        battleManager.AddUnitReference(spawnZone);
    }
}
