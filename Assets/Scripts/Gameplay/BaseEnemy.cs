using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    [SerializeField]
    protected float m_MoveSpeed = 10.0f;
    [SerializeField]
    protected float m_MaxHealth = 100;
    protected float m_CurHealth = 100;
    [SerializeField]
    protected float m_Damage = 1;
    [SerializeField]
    protected float m_AttackSpeed = 120;
    protected float m_AttackStage = 0;
    [SerializeField]
    protected float m_ArmorClass = 0;
    public enum ArmorType{HEAVY,LIGHT }
    [SerializeField]
    protected ArmorType m_ArmorType = ArmorType.LIGHT;

    protected bool m_Flying = false;

    protected bool m_CanDoNextAction = true;

    protected bool m_paused = false;
    public int m_ID;

    protected static EnemyObjectPool<BaseEnemy> pool = new EnemyObjectPool<BaseEnemy>();

    

    //实装自己的ai
    public abstract void DoAction();


    public void Start()
    {
        
        if (m_Flying)
        {

        }
    }

    public void OnEnable()
    {
        FindObjectOfType<CharacterManager>().AddUnitReference(this);
        
    }

    public void OnDisable()
    {
        
        try
        {
            //FindObjectOfType<CharacterManager>().RemoveUnitReference(this);
            //Moved to HitToDeath();
        }
        catch
        {

        }
    }

    public virtual void Hit(float damage, float str, int damageType)
    {
        m_CurHealth -= damage * (1.0f - m_ArmorClass / (m_ArmorClass + 10.0f));
        if (m_CurHealth < .0f)
        {
            HitToDeath();
        }
        Debug.Log(this + " now health is:" + m_CurHealth);
    }

    public virtual void Kill()
    {
        pool.Return(gameObject);
    }

    protected virtual void HitToDeath()
    {
        FindObjectOfType<CharacterManager>().RemoveUnitReference(this);
        //pool.Return(gameObject);
    }

    protected virtual void AfterDeath()
    {

    }

    protected virtual void BeforeClear()
    {

    }

    public virtual void ResetAll()
    {
        m_CurHealth = m_MaxHealth;
        m_CanDoNextAction = true;
    }

    public virtual GameObject Clone(Vector3 at,Quaternion to = new Quaternion())
    {
        //return Instantiate(gameObject, at, to);
        GameObject go = pool.Get(this,at,to);
        //不能生成后手改！
        //是agent的问题，不知道到底为啥
        //go.transform.position = at;
        //go.transform.rotation = to;
        return go;
    }

    protected virtual void ReturnToPool()
    {
        
        pool.Return(gameObject);
    }

    public void GamePause(bool pause)
    {
        
    }

    private void OnDestroy()
    {
        pool.Destory(gameObject);
    }
}
