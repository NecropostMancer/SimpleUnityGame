using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//投射粒子效果用
public class Bullet : Projectile
{
    //[SerializeField]
    //private ParticleSystem defaultEffect;

    private static Bullet instance;
    //private static ParticleSystem instanceP;
    public override void Shoot(Vector3 acc,float str)
    {
        float maxBias = 100;
        float randx = Random.Range(-1, 1) * (1 - acc.x);
        float randy = Random.Range(-1, 1) * (1 - acc.x);
        
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2 + maxBias * randx, Screen.height / 2 + maxBias * randy, Camera.main.nearClipPlane));
        //if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, ~(1 << 9)))
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000f,~LayerMask.GetMask("MovingObjectDetector")))
        {


            BaseEnemy enemy = hitInfo.collider.gameObject.GetComponentInParent<BaseEnemy>();
            if (enemy != null)
            {
                Debug.Log(hitInfo.collider.gameObject);
                if (hitInfo.collider.gameObject.CompareTag("Critical"))
                {
                    enemy.Hit(str * 40.0f, 1, 0);
                }
                enemy.Hit(str * 20.0f, 1, 0);
            }
            else
            {
                //defaultEffect.transform.position = hitInfo.point;
                //defaultEffect.transform.rotation.SetLookRotation(-hitInfo.normal);
                transform.position = hitInfo.point;
                transform.rotation.SetLookRotation(hitInfo.normal);
                //defaultEffect.Emit(1);
                transform.GetChild(0).GetComponent<ParticleSystem>().Emit(1);
            }
        }
    }

    public override Projectile InstantiateProj(Vector3 at, Quaternion q)
    {
        if(instance == null)
        {
            instance = Instantiate(gameObject).GetComponent<Bullet>();
        }
        return instance;
    }

    // Start is called before the first frame update
    void Awake()
    {
        //Instantiate(instanceP);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
