using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Projectile : MonoBehaviour
{
    [SerializeField]
    protected float damage=1f;
    
    public virtual Projectile InstantiateProj(Vector3 at,Quaternion q)
    {
        //TODO: obj pool
        
        
        return Instantiate(gameObject, at, q).GetComponent<Projectile>();
    }

    public virtual float getDamage()
    {
        return damage;
    }

    public abstract void shoot(Vector3 speed,float damageMult);


}
