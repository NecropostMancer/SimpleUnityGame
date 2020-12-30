using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Missile : Projectile
{
    Rigidbody rb;
    bool flying = true;
    [SerializeField]
    private AreaEffect effect;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (flying)
        {
            transform.forward =
        Vector3.Slerp(transform.forward, rb.velocity.normalized, Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        flying = false;
        OnHit(collision);
    }

    protected virtual void OnHit(Collision collision)
    {
        if (effect)
        {
            AreaEffect _ = Instantiate(effect, transform.position, new Quaternion());
            _.SetDamage(GetDamage());
        }

        Destroy(gameObject);
    }

    public override void Shoot(Vector3 speed,float damageMult)
    {
        rb.velocity = speed;
        damage *= damageMult;
    }
}
