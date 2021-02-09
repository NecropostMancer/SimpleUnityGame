using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//投射粒子效果用
public class Bullet : Projectile
{
   
    private class BulletPool : GameObjectPool<Bullet>
    {
        public override GameObject Get(Bullet bullet)
        {
            if (_objects.Count == 0)
            {
                curMax++;
                return Object.Instantiate(bullet.gameObject);
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
    private float m_ParticleTime;
    private ParticleSystem[] m_InstanceP = new ParticleSystem[2];
    private static string metaltag = "Metal";
    public override void Shoot(Vector3 acc,float str)
    {
        float maxBias = 100;
        float randx = Random.Range(-1, 1) * (1 - acc.x);
        float randy = Random.Range(-1, 1) * (1 - acc.x);
        
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2 + maxBias * randx, Screen.height / 2 + maxBias * randy, Camera.main.nearClipPlane));
        //if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, ~(1 << 9)))
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000f,~LayerMask.GetMask("MovingObjectDetector")))
        {
            Debug.DrawRay(gameObject.transform.position,ray.direction);

            BaseEnemy enemy = hitInfo.collider.gameObject.GetComponentInParent<BaseEnemy>();
            if (enemy != null)
            {
                
                if (hitInfo.collider.gameObject.CompareTag("Critical"))
                {
                    enemy.Hit(str * 2.0f, 1, 0);
                }
                enemy.Hit(str * 1.0f, 1, 0);
            }
            else
            {
                //defaultEffect.transform.position = hitInfo.point;
                //defaultEffect.transform.rotation.SetLookRotation(-hitInfo.normal);
                
                transform.position = hitInfo.point;
                transform.rotation.SetLookRotation(hitInfo.normal);
                //defaultEffect.Emit(1);
                if (hitInfo.collider.gameObject.tag == metaltag)
                { 
                    m_InstanceP[1].Emit(1);
                } else
                {
                    m_InstanceP[0].Emit(1);
                }
                
            }
        }
    }

    public override Projectile InstantiateProj(Vector3 at, Quaternion q)
    {
        if (sm_bulletPool == null)
        {
            sm_bulletPool = new BulletPool();
        }
        GameObject go = sm_bulletPool.Get(this);
        go.SetActive(true);
        return go.GetComponent<Projectile>();
        
    }

    // Start is called before the first frame update
    void Awake()
    {
        
        m_InstanceP[0] = transform.GetChild(0).GetComponent<ParticleSystem>();
        m_InstanceP[1] = transform.GetChild(0).GetComponent<ParticleSystem>();
        m_ParticleTime = 2f;
    }

    // Update is called once per frame
    float time;
    void Update()
    {
        time += Time.deltaTime;
        if(time > m_ParticleTime)
        {
            sm_bulletPool.Return(this.gameObject);
            gameObject.SetActive(false);
            time = 0;
        }
    }

    

    private void OnDestroy()
    {
        sm_bulletPool.Clear();
    }
}

