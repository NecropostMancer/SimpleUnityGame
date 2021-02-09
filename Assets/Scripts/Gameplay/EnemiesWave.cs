using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemiesWave
{
    [Serializable]
    public class EnemiesSquad
    {
        public int [] members = {0,0,0 };

        public void Copy(EnemiesSquad To)
        {

            To.members = (int[])members.Clone();
        }

        public EnemiesSquad Clone()
        {
            var clone = new EnemiesSquad();
            clone.members = (int[])members.Clone();
            return clone;
        }

    };

    public List<EnemiesSquad> Squads = new List<EnemiesSquad>();

    public void Copy(EnemiesWave To)
    {
        if (To == this) return;
        To.Squads.Clear();
        foreach(var array in Squads)
        {
            To.Squads.Add(array.Clone());
        }
        
    }

    public EnemiesWave Clone()
    {
        var a = new EnemiesWave();
        Copy(a);
        return a;
    }
}
