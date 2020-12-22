using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{

    protected float moveSpeed = 10.0f;
    protected float maxHealth = 100;
    protected float curHealth = 100;
    protected float damage = 10;
    protected float attackSpeed = 120;
    protected float attackStage = 0;
    protected float armorClass = 0;
    public enum ArmorType{HEAVY,LIGHT }
    protected ArmorType armorType = ArmorType.LIGHT;

    protected bool flying = false;

    protected bool canDoNextAction = true;

    //实装自己的ai
    public abstract void doAction();


    public void Start()
    {
        if (flying)
        {

        }
    }

    public virtual void Hit(float damage, float str, int damageType)
    {
        curHealth -= damage * (1.0f - armorClass / (armorClass + 10.0f));
        if (curHealth < .0f)
        {
            HitToDeath();
        }
        Debug.Log(this + " now health is:" + curHealth);
    }

    public virtual void Kill()
    {
        Destroy(gameObject);
    }

    protected virtual void HitToDeath()
    {
        Destroy(gameObject);
    }

}
