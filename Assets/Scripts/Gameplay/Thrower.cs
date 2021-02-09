using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
//投射粒子效果用
public class Thrower : Projectile
{
    private Rigidbody m_Rb;
    private class BulletPool : GameObjectPool<Thrower>
    {
        public override GameObject Get(Thrower bullet)
        {
            if (_objects.Count == 0)
            {
                curMax++;
                return Instantiate(bullet.gameObject);
            }
            else
            {

                _objects.TryTake(out GameObject go);

                return go;
            }
        }

        public void Clear()
        {

            if (_objects.Count != 0)
            {
                lock (new object())
                {

                    _objects = new System.Collections.Concurrent.ConcurrentBag<GameObject>();

                }
            }

        }
    }

    private static BulletPool sm_bulletPool;
    public override void Shoot(Vector3 speed, float damageMult)
    {
        m_Rb.velocity = speed;
        m_Damage *= damageMult;
        
    }

    public override Projectile InstantiateProj(Vector3 at, Quaternion q)
    {
        if (sm_bulletPool == null)
        {
            sm_bulletPool = new BulletPool();
        }
        GameObject go = sm_bulletPool.Get(this);
        go.transform.position = at;
        go.transform.rotation = q;
        go.SetActive(true);
        return go.GetComponent<Projectile>();
    }

    // Start is called before the first frame update
    void Awake()
    {
        m_Rb = GetComponent<Rigidbody>();
    }
    private float time;
    void Update()
    {
        time += Time.deltaTime;
        if (time > 1.25f)
        {
            sm_bulletPool.Return(this.gameObject);
            gameObject.SetActive(false);
            time = 0;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("MovingObjectDetector"))
        {
            BaseEnemy baseEnemy = other.gameObject.GetComponent<BaseEnemy>();
            baseEnemy.Hit(m_Damage,0,0);
        }
        
    }
    private void OnDestroy()
    {
        sm_bulletPool.Clear();
    }
}
