using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGen : MonoBehaviour
{
    // Start is called before the first frame update

    public Projectile projectilePrefab;
    [SerializeField]
    private float throwSpeed = 100.0f;
    [SerializeField]
    private float damageMult = 1.0f;
    private float curDamageMult;
    [SerializeField]
    private Transform aim;
    [SerializeField]
    bool isProjectile = true;
    [SerializeField]
    AreaEffect showBulletHitPoint;
    bool active = false;

    private readonly List<ParticleSystem> shootingParticles = new List<ParticleSystem>();


    public void Awake()
    {
        int cnt = transform.childCount;
        for (int i = 0; i < cnt; i++)
        {
            ParticleSystem ps;
            ps = transform.GetChild(i).GetComponent<ParticleSystem>();
            if (ps)
            {
                shootingParticles.Add(ps);
            }
        }
        //if missing default values
        if (projectilePrefab == null)
        {
            if (isProjectile)
            {
                //projectilePrefab = ((GameObject)Resources.Load("Assets/Prefab/DefaultProj.prefab")).GetComponent<Projectile>();
            }
            else
            {
                //projectilePrefab = ((GameObject)Resources.Load("Assets/Prefab/DefaultBullet.prefab")).GetComponent<Projectile>();
            }
        }
    }


    public void ShootProjectile(float accuracy)
    {
        if (!active)
        {
            return;
        }
        Vector3 dir = (transform.position - aim.position).normalized;
        if (isProjectile)
        {
            
            //strange rotating?
            Projectile a = projectilePrefab.InstantiateProj(transform.position, transform.rotation);
            a.Shoot(throwSpeed * dir,curDamageMult*damageMult);
        }
        else
        {
            //shoot regular dummy bullet
            
            projectilePrefab.InstantiateProj(transform.position, transform.rotation).Shoot(new Vector3(accuracy,0,0), curDamageMult * damageMult);
        }
        EmitParticles(1);
    }


    public void SetDamageBuff(float a)
    {
        curDamageMult = a;
    }

    private float GetTotalDamageBuff()
    {
        return curDamageMult * damageMult;
    }
    
    public void SetReady(bool a)
    {
        active = a;
    }

    private void EmitParticles(int cnt)
    {
        foreach(ParticleSystem ps in shootingParticles)
        {
            ps.Emit(cnt);
        }
    }
}
