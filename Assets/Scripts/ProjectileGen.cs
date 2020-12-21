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

    private List<ParticleSystem> shootingParticles = new List<ParticleSystem>();


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
    }


    public void shootProjectile()
    {
        if (!active)
        {
            return;
        }
        if (isProjectile)
        {
            Vector3 dir = (transform.position - aim.position).normalized;
            //strange rotating?
            Projectile a = projectilePrefab.InstantiateProj(transform.position, transform.rotation);
            a.shoot(throwSpeed * dir,curDamageMult*damageMult);
        }
        else
        {
            //shoot regular dummy bullet
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane));
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, ~(1 << 9)))
            {

                if (showBulletHitPoint)
                {
                    Instantiate(showBulletHitPoint, hitInfo.point, new Quaternion());
                }
                BaseEnemy enemy = hitInfo.collider.gameObject.GetComponent<BaseEnemy>();
                if (enemy != null)
                {
                    enemy.Hit(damageMult * curDamageMult * 20.0f, 1, 0);
                }
            }
        }
        EmitParticles(1);
    }


    public void setDamageBuff(float a)
    {
        curDamageMult = a;
    }

    private float getTotalDamageBuff()
    {
        return curDamageMult * damageMult;
    }
    
    public void setReady(bool a)
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
