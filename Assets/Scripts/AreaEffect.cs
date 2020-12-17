using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 为什么有这个类：
炸弹和炸弹爆炸毕竟不是一个事情。
 */
public class AreaEffect : MonoBehaviour
{
    bool run = false;
    //效果扩张，保持最大，收缩的帧数
    int expandFrame = 10;
    int holdFrame = 10;
    int declineFrame = 10;
    //伤害加成(可以手动设置，方便测试)
    public float damageMult = 1f;

    //这个爆炸将会对范围内对象造成多少伤害
    //-1的理由在 Impact协程处详述。
    private float damage = -1f;

    //爆炸除视觉效果外的实际影响范围
    float actualRadius = 30.0f;
    
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(Impact());
        StartCoroutine(ShowVisualEffect());
        
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
     */
    private IEnumerator Impact()
    {
        
        while(damage < 0)
        {
            yield return null;
        }
        _Impact();
    }

    public void _Impact()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, actualRadius,1<<10);
        foreach(Collider collider in colliders)
        {
            BaseEnemy enemy = collider.gameObject.GetComponent<BaseEnemy>();
            if (enemy != null)
            {
                enemy.Hit(damage * damageMult,1f,0);
            }
        }

        //castsphere here.
    }

    private IEnumerator ShowVisualEffect()
    {
        for(int i = 0;i < expandFrame; i++)
        {
            gameObject.transform.localScale = gameObject.transform.localScale * 1.1f;
            yield return null;
        }
        for (int i = 0; i < holdFrame; i++)
        {
            yield return null;
        }
        for (int i = 0; i < declineFrame; i++)
        {
            gameObject.transform.localScale = gameObject.transform.localScale * 0.5f;
            yield return null;
        }
        Destroy(gameObject);
    }

    public void setDamageMult(float a)
    {
        damageMult = a;
    }

    public void setDamage(float a)
    {
        damage = a;
    }
}
