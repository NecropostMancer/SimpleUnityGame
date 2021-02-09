using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySoldier : BaseEnemy
{
    [SerializeField]
    private float m_Courage = 10f;

    public bool m_IsLeader = false;
    private EnemySoldier m_MyLeader = null;

    private bool m_CloseEnough = false;
    private bool m_Safe = true;
    private bool m_LeaderTooFar = false;
    private bool m_Aimed = false;
    private bool m_Battle = false;

    private bool m_Dead = false;
    private Animator m_Animator;

    private float m_MinSpeed = 8.0f;

    private float m_MaxSpeed = 10.0f;

    private float m_MaxAngularSpeed = 1.0f;//deg/frame

    private Rigidbody m_ColliderVolume;
    [SerializeField]
    private PathFinder m_PathFinder;
    [SerializeField]
    private Vector3 m_SpeedVector;
    [SerializeField]
    private Vector3 m_CurGoal;
    private Vector3 m_FinalGoal;
    private Vector3 m_PlayerPlaceGoal;

    private Vector3 m_MoveStartFrom;

    protected Rigidbody[] bodies;
    private List<Vector3> rigPosition = new List<Vector3>();
    private List<Quaternion> rigRotation = new List<Quaternion>();
    [SerializeField]
    private Transform RigSpine1;

    [SerializeField]
    protected SkinnedMeshRenderer m_Skin;
    [SerializeField]
    protected MeshRenderer m_HoldingSkin;
    
    

    protected AudioSource m_AudioSource;
    [SerializeField]
    private AudioClip[] m_ImDamaged;
    [SerializeField]
    private AudioClip[] m_ImDead;
    // Start is called before the first frame update
    protected void Start()
    {
        base.Start();

        //SetNewMovementGoal(new Vector3(-727.55f, 182.8f, -979.46f));
        bodies = GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody rb in bodies)
        {
            rigPosition.Add(rb.gameObject.transform.position);
            rigRotation.Add(rb.gameObject.transform.rotation);
        }

        m_AudioSource = GetComponent<AudioSource>();
        AudioManager.instance.AudioRegister(m_AudioSource, AudioManager.AudioType.GameSFX);
        m_ColliderVolume = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
        m_Animator.enabled = true; // 开始时缓存一个tpose，因此是关闭的
        if (m_IsLeader)
        {
            m_CanDoNextAction = false;
            HireHenchman();
        }
        else
        {
            m_CanDoNextAction = true;
        }
    }
    private float m_DeadTimer;
    protected bool m_AllowBodyClear = true; 
    // Update is called once per frame
    protected void Update()
    {
        if (Time.timeScale < 0.001) return;
        //this.transform.rotation = Quaternion.Euler(0, Time.time * 40 % 360, 0);
        if (m_Dead) {
            m_DeadTimer += Time.deltaTime;
            if(m_DeadTimer > 5f)
            {
                if (m_AllowBodyClear)
                {
                    ReturnToPool();
                }
            }
            return; 
        }
        if (m_CanDoNextAction)
        {
            m_CanDoNextAction = false;
            //StartCoroutine(DoActionRandomDelay());
            DoAction();
        }
    }
    #region Action
    public override void DoAction()
    {
        Think();
        if (m_CloseEnough && !m_Safe)
        {
            Duck();
            return;
        }
        if (m_CloseEnough && m_Safe && !m_Aimed)
        {
            Aim();

            
        }
        if (m_Aimed)
        {
            
            Shoot();
            return;
            
        }
        if (!m_CloseEnough)
        {
            FindCurrentGoal();
            Move();
            return;
        }

        if (m_MyLeader && m_LeaderTooFar)
        {
            Follow();
            return;
        }
        Idle();

    }
    private delegate void BasicAction();
    
    private Vector3 lastGoal;
    private void Think()
    {
        if ((lastGoal - m_FinalGoal).sqrMagnitude > 1)
        {
            lastGoal = m_FinalGoal;
            if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Aiming SniperRifle"))
            {
                m_Animator.SetTrigger("EndAttack");
            }
            m_CloseEnough = false;
        }
        bool get = CharacterManager.instance.GetPlayerPos(out m_PlayerPlaceGoal);

        bool mayNeedAdvance = false;
        float timeSinceStop = 0f;
        if (get)
        {
            Vector3 delta = m_FinalGoal - m_PlayerPlaceGoal;
            if(delta.sqrMagnitude > m_Courage * m_Courage * 1.5)
            {
                mayNeedAdvance = true;
            }
            if (mayNeedAdvance)
            {
                m_FinalGoal = m_PlayerPlaceGoal + new Vector3(Random.Range(-m_Courage, m_Courage), 0, Random.Range(-m_Courage, m_Courage));
                if (Physics.Raycast(m_FinalGoal, -Vector3.up, out RaycastHit hit, 10f))
                {
                    m_FinalGoal = hit.point;
                    m_FinalGoal.y += 0.05f;
                    SetNewMovementGoal(m_FinalGoal);
                    m_CloseEnough = false;
                }
            }

        }
    }
    private int ShootCounter = 0; 
    private void Shoot()
    {
        ShootCounter++;
        if (ShootCounter > 250)
        {
            ShootCounter = 0;
            Debug.Log("shoot");
            m_Animator.SetTrigger("Shoot");
            CharacterManager.instance.AttackPlayer(m_Damage);
            m_Aimed = false;
        }
        
        m_CanDoNextAction = true;
    }
    private void Aim()
    {
        m_Animator.SetTrigger("StartAttack");
        Vector3 noY = m_PlayerPlaceGoal;
        noY.y = transform.position.y;
        transform.LookAt(noY, Vector3.up);
        Vector3 dis = m_PlayerPlaceGoal - RigSpine1.position;
        if(!Physics.Raycast(RigSpine1.position,dis,dis.magnitude + 1, 0))
        {
            m_Aimed = true;
        }
        m_CanDoNextAction = true;
    }
    //[SerializeField]
    //private Vector3 fix;
    //for animation rig
    private void LateUpdate()
    {
        if (m_Dead) return;
        AnimatorStateInfo info = m_Animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsTag("append"))
        {
            if (RigSpine1)
            {
                //RigSpine1.rotation = Quaternion.Euler(0, 0, 0);
                //Vector3 dir = (m_PlayerPlaceGoal - RigSpine1.transform.position).normalized;
                //Vector3 curDir = transform.forward;
                //RigSpine1.up = dir;
                //Quaternion fix = Quaternion.Euler(90, 0, 90);
                //RigSpine1.rotation = Quaternion.LookRotation(m_PlayerPlaceGoal-RigSpine1.position) * fix;
                
                RigSpine1.LookAt(m_PlayerPlaceGoal, Vector3.up);
                RigSpine1.Rotate(0,-68,-90);
            }
        }
    }

    private void FindCurrentGoal()
    {
        if(m_CurPathPointGen == null)
        {
            bool get = CharacterManager.instance.GetPlayerPos(out m_PlayerPlaceGoal);
            if (get)
            {
                m_FinalGoal = m_PlayerPlaceGoal + new Vector3(Random.Range(-m_Courage, m_Courage), 0, Random.Range(-m_Courage, m_Courage));
                if (Physics.Raycast(m_FinalGoal, -Vector3.up, out RaycastHit hit, 10f))
                {
                    m_FinalGoal = hit.point;
                    m_FinalGoal.y += 0.05f;
                    SetNewMovementGoal(m_FinalGoal);
                }

            }
        }
    }
    #endregion //Action
    #region Movement

    IEnumerator m_CurPathPointGen;
    private void Move()
    {
        
        if (m_CurPathPointGen == null)
        {
            //m_curPathPointGen = GetNewMovementSubGoal();
            //i dont know where to go!
            //use SetNewMovementGoal() first.
            m_CanDoNextAction = true;
            return;
        }
        if (m_CurPathPointGen.MoveNext())
        {
            m_Animator.ResetTrigger("EndMove");
            Vector3 to = m_CurGoal - transform.position;
            to.y = 0;
            GetComponent<Rigidbody>().isKinematic = false;
            //StartCoroutine(InterpolateMovementToFixedSpeed(speedVector, to.normalized * Random.Range(minSpeed, maxSpeed)));
            StartCoroutine(InterpolateMovementToFixedGoal(m_SpeedVector, to.normalized * Random.Range(m_MinSpeed, m_MaxSpeed), m_CurGoal));
            m_Animator.SetTrigger("StartMove");
        }
        else
        {
            m_Animator.ResetTrigger("StartMove");
            m_Animator.SetTrigger("EndMove");
            m_CanDoNextAction = true;
            thresMulti = 1f;
            m_SpeedVector = new Vector3();
            GetComponent<Rigidbody>().isKinematic = true;
            m_CloseEnough = true;
        }
        

    }

    private void Duck()
    {
        if (m_Battle)
        {
            m_Animator.SetTrigger("EndAttack");
        }
        DoMovementAnimation(Random.Range(8f, 10f));
        m_Animator.SetTrigger("StartMove");
        m_Safe = true;
        m_CanDoNextAction = true;
    }

    private void Idle()
    {
        //animator.SetTrigger("")
        m_CanDoNextAction = true;
    }

    private void Follow()
    {
        if (m_Battle)
        {
            m_Animator.SetTrigger("EndAttack");
        }
        m_Animator.SetTrigger("StartMove");
        m_CanDoNextAction = true;
    }
    
    private void HireHenchman()
    {
        
        m_CanDoNextAction = true;
    }

    public void SetNewMovementGoal(Vector3 to)
    {
        m_FinalGoal = to;
        m_CurPathPointGen = GetNewMovementSubGoal();
    }

    public void GetNewLeader(EnemySoldier leader)
    {
        m_MyLeader = leader;
    }

    private void DoMovementAnimation(float Speed)
    {
        m_Animator.SetFloat("Blend", (Speed-m_MinSpeed)/(m_MaxSpeed-m_MinSpeed));
        m_Animator.SetTrigger("StartMove");
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
            m_Animator.SetFloat("Blend", (fromSpeed.magnitude*(1-ratio) + toSpeed.magnitude*ratio) / (m_MaxSpeed - m_MinSpeed));
            m_SpeedVector = fromSpeed * (1 - ratio) + toSpeed * ratio;
            Quaternion q = new Quaternion();
            q.SetLookRotation(m_SpeedVector, Vector3.up);
            transform.localRotation = q;
            //this.transform.LookAt(transform.position+speedVector,Vector3.up);
            yield return null;
        }
        m_CanDoNextAction = true;
        yield break;
    }
    //public 
    IEnumerator InterpolateMovementToFixedGoal(Vector3 fromSpeed, Vector3 toSpeed, Vector3 toGoal)
    {
        if(fromSpeed.magnitude == 0)
        {
            fromSpeed = transform.forward * 0.1f;
        }
        //记录当前位置，判断是否卡住用
        lastPos = transform.position;
        delta = new Vector3();
        accumulateDelta = new Vector3();
        int softStuckTime = 0;
        int hardStuckTime = 0;
        
        //在速度末端和终点的直线上按时间插值作为视线落点
        float interTime = Random.Range(1, 3);
        float deltaTime = 0;
        //如果当前已经太快就减速?
        float fromSpeedValue = fromSpeed.magnitude;
        float toSpeedValue = toSpeed.magnitude;
        Vector3 line = toGoal - fromSpeed - lastPos;
        //Debug.DrawLine(transform.position + fromSpeed + new Vector3(0, 1, 0), toGoal + new Vector3(0, 1, 0), Color.red,999f);
        line.y = 0;
        while (true)
        {
            
            deltaTime += Time.deltaTime;
            float ratio = Mathf.SmoothStep(0, 1, deltaTime / interTime);
            float curSpeedValue = (fromSpeedValue * (1 - ratio) + toSpeedValue * ratio);
            float curSpeedRatio = curSpeedValue / (m_MaxSpeed - m_MinSpeed);
            m_Animator.SetFloat("Blend", curSpeedRatio);
            
            
            m_SpeedVector = fromSpeed * (1 - ratio) + toSpeed * ratio;
            Quaternion q = new Quaternion();
            //Vector3 finalDir = line * ratio + fromSpeed + lastPos - transform.position;
            Vector3 finalDir = (line * ratio + fromSpeed) * (1 - ratio) + ratio * (toGoal - transform.position);

            //Debug.DrawRay(transform.position + new Vector3(0,1,0), finalDir);
            m_SpeedVector = finalDir.normalized * curSpeedValue;
            finalDir.y = 0;
            q.SetLookRotation(finalDir, Vector3.up);
            transform.localRotation = q;
            //this.transform.LookAt(transform.position+speedVector,Vector3.up);
            if (ReachGoal(toGoal))
            {
                m_SpeedVector = m_SpeedVector.normalized * curSpeedValue;
                //Debug.Log("moche");
                break;
            }
            if (isSoftStuck)
            {
                softStuckTime++;
                if (softStuckTime > 120)
                {
                    //Debug.Log("im stuck!");
                    SetNewMovementGoal(m_FinalGoal);
                    break;
                }
            }
            else
            {
                
            }
            if (thresMulti > 4)
            {
                ForceTeleport(3);
                thresMulti-- ;
            }
            RecordActualSpeed(transform.position);
            RecordMovingProgress(transform.position);
            yield return null;
        }
        m_CanDoNextAction = true;
        yield break;
    }
    [SerializeField]
    private float thresMulti = 1f;
    private bool ReachGoal(Vector3 goalWorldPos, float thres = 0.5f)
    {
        Vector3 delta = transform.position - goalWorldPos;
        delta.y /= 15;
        if (delta.sqrMagnitude < thres * thres * thresMulti * thresMulti)
        {
            

            return true;
        }
        else
        {
            return false;
        }
    }
    private void ForceTeleport(float dis)
    {
        transform.position = transform.position + transform.forward * dis + Vector3.up;
    }
    
    bool isSoftStuck = false;// recalc path
    bool isHardStuck = false;// expand final
    Vector3 lastPos;
    Vector3 delta;
    float time = 0f;
    public List<float> avgProgressSec = new List<float>();
    float tmp;
    private void RecordActualSpeed(Vector3 currentPos)
    {
        delta = currentPos - lastPos;
        lastPos = currentPos;
        float dot = Vector3.Dot(delta.normalized, m_SpeedVector.normalized);
        if (dot > 0.01)
        {
            //Debug.Log(dot);
        }
        if(dot < 0.8)
        {
            isSoftStuck = true;
            
        }
        else
        {
            isSoftStuck = false;
        }
    }
    Vector3 accumulateDelta = new Vector3();
    //只是能用
    private void RecordMovingProgress(Vector3 currentPos)
    {
        time += Time.deltaTime;
        
        
        accumulateDelta += delta;
        if (time > 1f)
        {
            avgProgressSec.Add(accumulateDelta.magnitude);
            accumulateDelta.x = accumulateDelta.y = accumulateDelta.z = 0;
            time = 0;
        }
        //如果极差大于最小值的50%?
        if(avgProgressSec.Count > 1)
        {
            if(avgProgressSec[1] <0.6f)
            {
                isHardStuck = true;
                thresMulti += 1;
            }
            else
            {
                isHardStuck = false;
                if (thresMulti >= 2)
                {
                    thresMulti -= 1;
                }
                
            }

            avgProgressSec.Clear();
        }
        /*
        if((currentPos - m_FinalGoal).sqrMagnitude - (lastPos - m_FinalGoal).sqrMagnitude > 0) //0.1f
        {
            isHardStuck = true;
        }
        else
        {
            isHardStuck = false;
        }*/

    }
    
    private void ForceRotate(float Y)
    {
        this.transform.rotation = Quaternion.Euler(0, Y, 0);
    }

    private IEnumerator GetNewMovementSubGoal()
    {
        if (!m_PathFinder)
        {
            return RandomMovement();
        }
        else
        {
            return DefaultPathFinder();
        }
    }

    IEnumerator RandomMovement()
    {
        while (true)
        {
            m_CurGoal = transform.position + new Vector3(Random.Range(-40, 40), 0, Random.Range(-40, 40));
            yield return m_CurGoal;
        }
    }

    IEnumerator DefaultPathFinder()
    {
        Vector3[] corners = m_PathFinder.calc(m_FinalGoal,GetComponent<NavMeshAgent>()).corners;
        if(corners.Length > 0)
        {
            m_FinalGoal = corners[corners.Length - 1];
        }
        else
        {
            m_FinalGoal = transform.position;
        }
        foreach (Vector3 vector3 in corners)
        {
            m_CurGoal = vector3;
            //Debug.Log(vector3);
            yield return vector3;
        }
        yield break;
    }
    private void OnAnimatorMove()
    {
        Vector3 velocity = m_Animator.deltaPosition / Time.fixedDeltaTime;
        Vector3 fallVelocity = m_ColliderVolume.velocity;
        fallVelocity.x = 0;
        fallVelocity.z = 0;
        m_ColliderVolume.velocity = velocity + fallVelocity;
        
        m_Animator.ApplyBuiltinRootMotion();
    }

    #endregion //Movement
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


    private float m_HurtChance = 0.3f;
    public override void Hit(float damage, float str, int damageType)
    {
        if (m_Dead) return;
        base.Hit(damage, str, damageType);
        if (m_Animator.GetCurrentAnimatorStateInfo(1).IsName("Default"))
        {
            m_Animator.SetTrigger("DamageTrigger");
            m_Animator.SetInteger("Damage", Random.Range(1, 4));
        }
        if (m_ImDamaged == null || m_ImDamaged.Length == 0) return;
        if (Random.Range(0f, 1f) < m_HurtChance)
        {
            m_AudioSource.clip = m_ImDamaged[Random.Range(0, m_ImDamaged.Length)];
            m_AudioSource.Play();
        }
    }
    
    protected override void HitToDeath()
    {
        base.HitToDeath();
        m_Dead = true;
        StopAllCoroutines();
        m_CanDoNextAction = false;
        m_Animator.SetTrigger("Killed");
        //ragdoll.
        //not work?
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        
        
        Utils.SetLayerRecursively(gameObject, 0);//重新设定到世界层，让布娃娃可以碰撞。
        foreach (Rigidbody rb in bodies)
        {
            //rb.maxDepenetrationVelocity = 0.1f;
            rb.isKinematic = false;
            
        }
        m_ColliderVolume.isKinematic = true;
        m_Animator.enabled = false;
        if (m_ImDead == null || m_ImDead.Length == 0) return;
        
        m_AudioSource.clip = m_ImDead[Random.Range(0, m_ImDead.Length)];
        m_AudioSource.Play();
        AfterDeath();
    }

    protected override void AfterDeath()
    {
        base.AfterDeath();
        
        
    }

    protected void OnDisable()
    {
        base.OnDisable();

        m_ColliderVolume.isKinematic = true;
        for (int i = 0; i < bodies.Length; i++)
        {
            bodies[i].isKinematic = true;
            //bodies[i].gameObject.SetActive(true);
            bodies[i].gameObject.transform.position = rigPosition[i];
            bodies[i].gameObject.transform.rotation = rigRotation[i]; //懂了
            bodies[i].useGravity = true;
        }
        
        
    }

    protected void OnEnable()
    {
        base.OnEnable();
        if (m_Dead)
        {
            Utils.SetLayerRecursively(gameObject, 10);
            gameObject.layer = 11;
            gameObject.GetComponent<NavMeshAgent>().enabled = true;
            gameObject.GetComponent<CapsuleCollider>().enabled = true;

            /*
            foreach (Rigidbody rb in bodies)
            {
                //rb.maxDepenetrationVelocity = 0.1f;
                rb.isKinematic = true;
                rb.gameObject.SetActive(true);
                
            }
            m_ColliderVolume.isKinematic = true;
            */
            m_Animator.enabled = true;
            m_Animator.Play("Wary Rifle", -1);
            m_Animator.Play("Default", 1);
            
            
        }
        ResetAll();
    }

    public override void ResetAll()
    {
        base.ResetAll();
        m_IsLeader = false;
        m_MyLeader = null;

        m_CloseEnough = false;
        m_Safe = true;
        m_LeaderTooFar = false;
        m_Aimed = false;
        m_Battle = false;

        m_Dead = false;
        m_DeadTimer = 0;
        m_SpeedVector = new Vector3();
        m_FinalGoal = new Vector3();
        m_CurPathPointGen = null;

        isSoftStuck = false;// recalc path
        isHardStuck = false;// expand final
        lastPos = new Vector3();
        delta = new Vector3();
        time = 0f;
        avgProgressSec = new List<float>();
        lastGoal = new Vector3();
    }
    //现在所有baseenemy公用一个池子，简单拉出来老实例无法确认具体属性和变种。
    //在这里重新初始化。
    //这个完成后要手动enable。
    public override GameObject Clone(Vector3 at, Quaternion to = default)
    {
        GameObject go = base.Clone(at, to);
        EnemySoldier profile = go.GetComponent<EnemySoldier>();
        profile.m_MoveSpeed = m_MoveSpeed;
        profile.m_MaxHealth = m_MaxHealth;
        profile.m_Damage = m_Damage;
        profile.m_AttackSpeed = m_AttackSpeed;
        profile.m_ArmorClass = m_ArmorClass;
        profile.m_ArmorType = m_ArmorType;
        profile.m_Courage = m_Courage;
        profile.m_Skin.material = m_Skin.sharedMaterial;
        profile.m_HoldingSkin.material = m_HoldingSkin.sharedMaterial;
        go.transform.name = transform.name;
        return go;
    }

    protected bool IsDead()
    {
        return m_Dead;
    }
}
