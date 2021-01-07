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

    private float maxAngularSpeed = 1.0f;//deg/frame

    private Rigidbody colliderVolume;
    public PathFinder pathFinder;
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
        colliderVolume = GetComponent<Rigidbody>();
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
        GetNewMovementGoal();
        Vector3 to = curGoal - transform.position;
        to.y = 0;
        StartCoroutine(InterpolateMovementToFixedSpeed(speedVector, to.normalized * Random.Range(minSpeed, maxSpeed)));
        //StartCoroutine(InterpolateMovementToFixedGoal(speedVector, to.normalized * Random.Range(minSpeed, maxSpeed), curGoal));
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
        Utils.SetLayerRecursively(gameObject, 0);//重新设定到世界层，让布娃娃可以碰撞。
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

    IEnumerator InterpolateMovementToFixedSpeed(Vector3 fromSpeed,Vector3 toSpeed)
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

    IEnumerator InterpolateMovementToFixedGoal(Vector3 fromSpeed, Vector3 toSpeed, Vector3 toGoal)
    {
        //在速度末端和终点的直线上按时间插值作为视线落点
        float interTime = Random.Range(1, 3);
        float deltaTime = 0;
        //如果当前已经太快就减速?

        
        while (true)
        {
            
            deltaTime += Time.deltaTime;
            //float ratio = Mathf.SmoothStep(0, 1, deltaTime / interTime);
            //float curSpeed = (fromSpeed.magnitude * (1 - ratio) + toSpeed.magnitude * ratio) / (maxSpeed - minSpeed);
            //animator.SetFloat("Blend", curSpeed);
            
            
            //speedVector = fromSpeed * (1 - ratio) + toSpeed * ratio;
            Quaternion q = new Quaternion();
            Vector3 finalDir = curGoal - transform.position;
            
            q.SetLookRotation(finalDir, Vector3.up);
            transform.localRotation = q;
            //this.transform.LookAt(transform.position+speedVector,Vector3.up);
            if (ReachGoal(toGoal))
            {
                //speedVector = speedVector.normalized * curSpeed;
                break;
            }
            yield return null;
        }
        canDoNextAction = true;
        yield break;
    }

    private bool ReachGoal(Vector3 goalWorldPos, float thres = 0.5f)
    {
        if((transform.position - goalWorldPos).sqrMagnitude < thres * thres)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ForceRotate(float Y)
    {
        this.transform.rotation = Quaternion.Euler(0, Y, 0);
    }

    private void GetNewMovementGoal()
    {
        if (!pathFinder)
        {
            curGoal = transform.position + new Vector3(Random.Range(-40, 40), 0, Random.Range(-40, 40));
        }
        else
        {
            //Vector3[] corners = pathFinder.calc(transform.position, new Vector3(-728.3952f, 188, -948)).corners;
            Vector3[] corners = pathFinder.calc(transform.position, new Vector3(-726f, 182.8f, -970)).corners;
            foreach (Vector3 vector3 in corners)
            {
                Debug.Log(vector3);
                //Debug.DrawLine
            }
            //Debug.Log();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject);
    }

    private void OnCollisionStay(Collision collision)
    {
        
    }

    private void OnAnimatorMove()
    {
        Vector3 velocity = animator.deltaPosition / Time.fixedDeltaTime;
        Vector3 fallVelocity = colliderVolume.velocity;
        fallVelocity.x = 0;
        fallVelocity.z = 0;
        colliderVolume.velocity = velocity + fallVelocity;
        
        animator.ApplyBuiltinRootMotion();
    }
}
