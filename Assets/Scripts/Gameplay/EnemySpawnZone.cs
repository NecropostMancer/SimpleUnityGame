using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnZone : SpawnZone
{
    protected List<BaseEnemy> baseEnemies = new List<BaseEnemy>();
    [SerializeField]
    private GameObject genEffect;
    public enum Tag {SOLDIER,TANK,AIR,PART_TIME,ONE_SHOT};

    //PART_TIME: 在progress正好为所需值的时候才活跃
    //ONE_SHOT: 只生效一次
    public int m_DifficultyLevel = 0;
    public int m_ProgressLevel = 0;
    public List<Tag> m_Tags = new List<Tag>();
    public int order;
    protected override void GenEntity()
    {
        GameObject baseEnemy = g_Products[Random.Range(0, g_Products.Count)].GetComponent<BaseEnemy>().Clone(Random3DPoint());
        AddToList(baseEnemy);
        baseEnemy.gameObject.SetActive(true);
        if (genEffect)
            Instantiate(genEffect,transform.position,transform.rotation,transform);
    }

    protected override void GenEntity(int i)
    {
        GameObject baseEnemy = g_Products[i % g_Products.Count].GetComponent<BaseEnemy>().Clone(Random3DPoint());
        AddToList(baseEnemy);
        baseEnemy.gameObject.SetActive(true);
        if(genEffect)
            Instantiate(genEffect, transform.position, transform.rotation, transform);
    }

    protected override void Clearlist()
    {
        baseEnemies.RemoveAll(i => i == null || i.gameObject.activeInHierarchy == false);
    }

    protected override void AddToList(GameObject go)
    {
        baseEnemies.Add(go.GetComponent<BaseEnemy>());
    }

    protected override bool CanGen()
    {
        if (autoGen)
        {
            return Random.Range(0, 30) > 27f && baseEnemies.Count < m_SpawnLimit;
        }
        else
        {
            if (Random.Range(0, 30) > 27f && baseEnemies.Count < m_SpawnLimit && requiredGenNum > 0)
            {
                requiredGenNum--;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public override void Gen(int num)
    {
        base.Gen(num);
    }
    public override void GenBrust(int num)
    {
        //base.GenBrust(num);
        for(int i = 0; i < num; i++)
        {
            GenEntity();
        }
    }
}
