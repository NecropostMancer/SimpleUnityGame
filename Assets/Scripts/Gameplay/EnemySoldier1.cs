using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoldier1 : BaseEnemy
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

    private float maxAngularSpeed = 1.0f;//deg/frame

    private Rigidbody colliderVolume;

    [SerializeField]
    private Vector3 speedVector;
    [SerializeField]
    private Vector3 curGoal;

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
        //this.transform.rotation = Quaternion.Euler(0, Time.time * 40 % 360, 0);
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
        //ChooseDir();
        //DoMovementAnimation(Random.Range(1f,10f));
        GetNewMovementGoal();
        Vector3 to = curGoal - transform.position;
        to.y = 0;
        StartCoroutine(InterpolateMovement(speedVector, to.normalized * Random.Range(minSpeed, maxSpeed)));
        animator.SetTrigger("StartMove");

        //canDoNextAction = true;
    }

    private void Duck()
    {
        if (battle)
        {
            animator.SetTrigger("EndAttack");
        }
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

    private void SetMoveDir(float y)
    {
        this.transform.rotation = Quaternion.Euler(0, y, 0);
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
        //not work?
        Destroy(gameObject.GetComponent<CapsuleCollider>());
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

    IEnumerator InterpolateMovement(Vector3 fromSpeed,Vector3 toSpeed)
    {
        float interTime = Random.Range(2, 5);
        float deltaTime = 0;
        bool stillNeedRotate = false;
        //float degDelta = 
        while (deltaTime < interTime || stillNeedRotate)
        {
            deltaTime += Time.deltaTime;
            float ratio = Mathf.SmoothStep(0, 1, deltaTime / interTime);
            animator.SetFloat("Blend", (fromSpeed.magnitude*(1-ratio) + toSpeed.magnitude*ratio) / (maxSpeed - minSpeed));
            speedVector = fromSpeed * (1 - ratio) + toSpeed * ratio;
            Quaternion q = new Quaternion();
            q.SetLookRotation(speedVector, Vector3.up);
            transform.localRotation = q;
            //this.transform.LookAt(transform.position+speedVector,Vector3.up);
            yield return null;
        }
        canDoNextAction = true;
        yield break;
    }
    private void ForceRotate(float Y)
    {
        this.transform.rotation = Quaternion.Euler(0, Y, 0);
    }

    private void GetNewMovementGoal()
    {
        curGoal = transform.position + new Vector3(Random.Range(-40, 40), 0, Random.Range(-40, 40));
    }

    private void OnCollisionEnter(Collision collision)
    {
        //.Log(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject);
    }

    private void OnAnimatorMove()
    {
        Vector3 velocity = animator.deltaPosition / Time.fixedDeltaTime;
        Debug.Log("speed" + velocity);
        transform.parent.GetComponent<Rigidbody>().velocity = velocity;
        //animator.ApplyBuiltinRootMotion();
    }
}
