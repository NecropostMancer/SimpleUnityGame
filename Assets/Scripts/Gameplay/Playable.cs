using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//子类沙盒
public class Playable : MonoBehaviour
{
    public virtual void Awake()
    {
        if (characterManager == null)
        {
            characterManager = CharacterManager.instance;
        }
    }


    private static CharacterManager characterManager;

    protected void SendUICommand(UICommand cmd) {

        //characterManager.SendUICommand(cmd);
    }

    protected void AddUnitReference(BaseEnemy baseEnemy)
    {
        characterManager.AddUnitReference(baseEnemy);
    }

    protected void RemoveUnitReference(BaseEnemy baseEnemy)
    {
        characterManager.RemoveUnitReference(baseEnemy);
    }

    protected void AddUnitReference(SpawnZone spawnZone)
    {
        characterManager.AddUnitReference(spawnZone);
    }
}
