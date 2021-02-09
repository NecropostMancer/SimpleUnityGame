using System.Collections.Generic;
using UnityEngine;

public class ProjectileGen : MonoBehaviour
{
    // Start is called before the first frame update

    public Projectile g_ProjectilePrefab;
    [SerializeField]
    private float m_ThrowSpeed = 100.0f;
    [SerializeField]
    private float m_DamageMult = 1.0f;
    private float m_CurDamageMult;
    [SerializeField]
    private Transform m_Aim;
    [SerializeField]
    private bool m_IsProjectile = true;
    [SerializeField]
    private AreaEffect m_ShowBulletHitPoint;
    private bool m_Active = false;

    private readonly List<ParticleSystem> m_ShootingParticles = new List<ParticleSystem>();


    public void Awake()
    {
        int cnt = transform.childCount;
        for (int i = 0; i < cnt; i++)
        {
            ParticleSystem ps;
            ps = transform.GetChild(i).GetComponent<ParticleSystem>();
            if (ps)
            {
                m_ShootingParticles.Add(ps);
            }
        }
        //if missing default values
        if (g_ProjectilePrefab == null)
        {
            if (m_IsProjectile)
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
        if (!m_Active)
        {
            return;
        }
        Vector3 dir = (transform.position - m_Aim.position).normalized;
        if (m_IsProjectile)
        {
            
            //strange rotating?
            Projectile a = g_ProjectilePrefab.InstantiateProj(transform.position, transform.rotation);
            a.Shoot(m_ThrowSpeed * dir,m_CurDamageMult*m_DamageMult);
        }
        else
        {
            //shoot regular dummy bullet
            
            g_ProjectilePrefab.InstantiateProj(transform.position, transform.rotation).Shoot(new Vector3(accuracy,0,0), m_CurDamageMult * m_DamageMult);
        }
        EmitParticles(1);
    }


    public void SetDamageBuff(float a)
    {
        m_CurDamageMult = a * GameAssetsManager.instance.GetSave().attackBonus;
    }

    private float GetTotalDamageBuff()
    {
        return m_CurDamageMult * m_DamageMult;
    }
    
    public void SetReady(bool a)
    {
        m_Active = a;
    }

    private void EmitParticles(int cnt)
    {
        foreach(ParticleSystem ps in m_ShootingParticles)
        {
            ps.Emit(cnt);
        }
    }
}
