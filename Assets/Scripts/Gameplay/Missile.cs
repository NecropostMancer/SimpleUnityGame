using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Missile : Projectile
{
    private Rigidbody m_Rb;
    private bool m_Flying = true;
    [SerializeField]
    private AreaEffect m_Effect;

   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        m_Rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Flying)
        {
            transform.forward =
        Vector3.Slerp(transform.forward, m_Rb.velocity.normalized, Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        m_Flying = false;
        OnHit(collision);
    }

    protected virtual void OnHit(Collision collision)
    {
        if (m_Effect)
        {
            AreaEffect _ = Instantiate(m_Effect, transform.position, new Quaternion());
            _.SetDamage(GetDamage());
        }

        Destroy(gameObject);
    }

    public override void Shoot(Vector3 speed,float damageMult)
    {
        m_Rb.velocity = speed;
        m_Damage *= damageMult;
    }
}
