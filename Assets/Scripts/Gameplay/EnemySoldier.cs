using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoldier : BaseEnemy
{

    private float courage = 10f;

    public bool isLeader = false;
    private EnemySoldier myLeader = null;

    private bool closeEnough = false;
    private bool safe = false;
    private bool leaderTooFar = false;
    private bool aimed = false;
    private bool battle = false;

    private bool dead = false;
    private Animator animator;

    private float minSpeed = 1.0f;

    private float maxSpeed = 10.0f;

    public override void DoAction()
    {
        if (closeEnough && !safe)
        {
            Duck();
            return;
        }
        if (closeEnough && safe)
        {
            Aim();
            
            return;
        }
        if (aimed)
        {
            Shoot();
        }
        if (!closeEnough)
        {
            Move();
            return;
        }

        if (myLeader && leaderTooFar)
        {
            Follow();
            return;
        }
        Idle();

    }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        if (isLeader)
        {
            canDoNextAction = false;
            HireHenchman();
        }
        else
        {
            canDoNextAction = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dead) return;
        if (canDoNextAction)
        {
            canDoNextAction = false;
            StartCoroutine(DoActionRandomDelay());
            
        }
    }

    IEnumerator DoActionRandomDelay()
    {
        for(int i = 0; i < (int)Random.Range(200, 600); i++)
        {
            yield return null;
        }
        DoAction();
    }

    private void Shoot()
    {
        animator.SetTrigger("Shoot");
        canDoNextAction = true;
    }
    private void Aim()
    {
        animator.SetTrigger("StartAttack");
        canDoNextAction = true;
    }
    private void Move()
    {
        ChooseDir();
        DoMovementAnimation(Random.Range(1f,10f));
        animator.SetTrigger("StartMove");

        canDoNextAction = true;
    }

    private void Duck()
    {
        if (battle)
        {
            animator.SetTrigger("EndAttack");
        }
        ChooseDir();
        DoMovementAnimation(Random.Range(8f, 10f));
        animator.SetTrigger("StartMove");
        canDoNextAction = true;
    }

    private void Idle()
    {
        //animator.SetTrigger("")
        canDoNextAction = true;
    }

    private void Follow()
    {
        if (battle)
        {
            animator.SetTrigger("EndAttack");
        }
        animator.SetTrigger("StartMove");
        canDoNextAction = true;
    }
    
    private void HireHenchman()
    {
        
        canDoNextAction = true;
    }

    public void GetNewLeader(EnemySoldier leader)
    {
        myLeader = leader;
    }

    private void DoMovementAnimation(float Speed)
    {
        animator.SetFloat("Blend", (Speed-minSpeed)/(maxSpeed-minSpeed));
        animator.SetTrigger("StartMove");
    }

    private void ChooseDir()
    {
        
       this.transform.rotation = Quaternion.Euler(0, Random.Range(-180f, 180f), 0);
    }

    public override void Hit(float damage, float str, int damageType)
    {
        if (dead) return;
        base.Hit(damage, str, damageType);
        if (animator.GetCurrentAnimatorStateInfo(1).IsName("Default"))
        {
            animator.SetTrigger("DamageTrigger");
            animator.SetInteger("Damage", Random.Range(1, 4));
        }

    }

    protected override void HitToDeath()
    {
        //base.HitToDeath();
        dead = true;
        StopAllCoroutines();
        canDoNextAction = false;
        animator.SetTrigger("Killed");
        //ragdoll.
        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in bodies)
        {
            //rb.maxDepenetrationVelocity = 0.1f;
            rb.isKinematic = false;
            
        }
        animator.enabled = false;
        StartCoroutine(Clear());
    }

    IEnumerator Clear()
    {
        yield return new WaitForSeconds(7);
        base.HitToDeath();
    }


}
