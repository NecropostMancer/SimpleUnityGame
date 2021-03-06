﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Projectile : MonoBehaviour
{
    [SerializeField]
    protected float m_Damage=1f;
    
    public virtual Projectile InstantiateProj(Vector3 at,Quaternion q)
    {
        //TODO: obj pool
        
        
        return Instantiate(gameObject, at, q).GetComponent<Projectile>();
    }

    public virtual float GetDamage()
    {
        return m_Damage;
    }

    public abstract void Shoot(Vector3 speed,float damageMult);


}
