using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 为什么有这个类：
炸弹和炸弹爆炸毕竟不是一个事情。

TODO: 把粒子安上去.
 */
public class AreaEffect : MonoBehaviour
{
    private bool m_Run = false;
    
    //效果扩张，保持最大，收缩的帧数
    private int m_ExpandFrame = 0;
    private int m_HoldFrame = 10;
    private int m_DeclineFrame = 0;
    
    //伤害加成(可以手动设置，方便测试)
    public float m_DamageMult = 1f;

    //这个爆炸将会对范围内对象造成多少伤害
    //-1的理由在 Impact协程处详述。
    private float m_Damage = -1f;

    //爆炸除视觉效果外的实际影响范围
    [SerializeField]
    float m_ActualRadius = 2.5f;

    private AudioSource m_AudioSource;

    // Start is called before the first frame update
    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
        AudioManager.instance.AudioRegister(m_AudioSource, AudioManager.AudioType.GameSFX);
        Impact();
        //StartCoroutine(ShowVisualEffect());
        m_ActualRadius *= transform.lossyScale.magnitude;
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
     伤害的计算方式涉及到projectile类和weapon类的两个乘数，
    但是目前没有一个好办法在Instantiate的同时传入这个乘积，
    只能在Instantiate后再调一个函数，
    所以用一个协程循环检查，如果这个乘积传入了，才开始实际
    计算伤害，可能会有卡死的问题，以后再想想。
    好像多余了，貌似可以保证发生在一帧里面
     */
    private void Impact()
    {

        /*transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        while(m_Damage < 0)
        {
            yield return null;
        }
        _Impact();*/
        m_AudioSource.Play();
        //不能放在oncollision里，因为所有的角色都是kinematic的
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_ActualRadius, 1 << 11);
        foreach (Collider collider in colliders)
        {
            BaseEnemy enemy = collider.gameObject.GetComponent<BaseEnemy>();
            if (enemy != null)
            {
                enemy.Hit(m_Damage * m_DamageMult, 1f, 0);
                //StartCoroutine(ApplyDelayExplosionForce(collider.transform.GetChild(1).GetComponent<Rigidbody>())); //waiting for a re-do

            }
        }
        Destroy(gameObject, 10);
    }


    IEnumerator ApplyDelayExplosionForce(Rigidbody rb)
    {
        yield return new WaitForSeconds(0.1f);
        //rb.AddExplosionForce(1000, transform.position, actualRadius);
        rb.velocity = new Vector3(0, 12, 0);
    }
    public void SetDamageMult(float a)
    {
        m_DamageMult = a;
    }

    public void SetDamage(float a)
    {
        m_Damage = a;
    }

}
