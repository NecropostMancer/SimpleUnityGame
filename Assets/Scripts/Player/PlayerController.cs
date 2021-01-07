using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//玩家的操作。
public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update


    Vector3 worldPos;
    [SerializeField]
    Transform pivot;
    [SerializeField]
    Transform pivot2;

    [SerializeField]
    float VSpeed = 200f;
    [SerializeField]
    float HSpeed = 300f;
    [SerializeField]
    float speedMult = 2.0f;
    [SerializeField]
    bool _lockCursor = true;
    [SerializeField]
    float aimmingFOV = 30f;
    [SerializeField]
    float normalFOV = 60f;
    [SerializeField]
    Camera FirstCamera;

    private float settingCurFov;

    public bool infAmmo = false;

    public Vector2 clampDeg = new Vector2(0, 180);

    bool lockCursor
    {
        set
        {
            if (value)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
            _lockCursor = value;
        }
        get
        {
            return _lockCursor;
        }
    }

    [SerializeField]
    private WeaponSlot weaponSlot;

    private BattleUIBundle battleUIBundle;

    void Start()
    {
        worldPos = pivot.position;
        lastDiag = new Vector2();
        if(lockCursor)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        settingCurFov = FirstCamera.fieldOfView;
        battleUIBundle = GameObject.FindGameObjectWithTag("UIBundle").GetComponent<BattleUIBundle>();
        if (clampDeg.x < 1)
        {
            clampDeg.x = 1;
        }
        if (clampDeg.y > 179)
        {
            clampDeg.y = 179;
        }
    }
    Vector2 lastDiag;
    bool canResume = false;
    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Mouse Y")* Time.deltaTime;
        float vertical = Input.GetAxis("Mouse X") * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            WeaponSwitch(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            WeaponSwitch(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            WeaponSwitch(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            WeaponSwitch(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            WeaponSwitch(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            WeaponSwitch(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            WeaponSwitch(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            WeaponSwitch(7);
        }


        if (Input.GetMouseButtonDown(0))
        {
            weaponSlot.m_currentWeapon.Fire(true);
            
        }
        else if (Input.GetMouseButtonUp(0))
        {
            weaponSlot.m_currentWeapon.Fire(false);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            weaponSlot.m_currentWeapon.Reload();
        }
        if(coroutineRunning == false){
            if (Input.GetMouseButtonDown(1))
            {
                StartCoroutine(AimingOn());

            }
            else if (Input.GetMouseButtonUp(1))
            {
                StartCoroutine(AimingOff());
                //animator.SetTrigger("AimingOff");

            }
        }
        else
        {
            
        }
        //transform

        

        Vector2 delta = new Vector2(horizontal * HSpeed * -1 * speedMult, vertical * VSpeed * speedMult);
        
        float angle = (pivot2.eulerAngles.x + 90f)%360;

        if(angle + delta.x < clampDeg.x)
        {
            delta.x = 0;
        }
        else if(angle + delta.x > clampDeg.y)
        {
            delta.x = 0;
        }



        //delta.x = angle + delta.x < 280f? 0 : delta.x;

        //Debug.Log(angle);
        
        pivot.Rotate(new Vector3(0f, delta.y, 0f));
        pivot2.Rotate(new Vector3(delta.x, 0f , 0f));

        //do recoil
        
        if(weaponSlot.m_currentWeapon.GetRecoilStr(out Quaternion str))
        {
            FirstCamera.transform.localRotation = Quaternion.Slerp(FirstCamera.transform.localRotation, str, 0.8f);
            //FirstCamera.transform.localRotation = str;
            recoving = true;
        }
        
        else
        {
            if (recoving)
            {
                FirstCamera.transform.localRotation = Quaternion.Slerp(FirstCamera.transform.localRotation, str, 0.6f);
                if(FirstCamera.transform.localRotation.eulerAngles.magnitude < 1)
                {
                    recoving = false;
                }
            }
        }
        
        //debug
        if (infAmmo)
        {
            weaponSlot.m_currentWeapon.infAmmo = true;
        }
        else
        {
            weaponSlot.m_currentWeapon.infAmmo = false;
        }
        
    }
    bool recoving = false;
    Quaternion str = Quaternion.Euler(0, 0, 0);
    void WeaponSwitch(int index)
    {
        weaponSlot.Switch(index);
        
    }
    
    private float goalFov = 30.0f;

    private bool coroutineRunning = false;
    IEnumerator AimingOn()
    {
        coroutineRunning = true;
        //all aiming animations are 4 frames.
        float curFov = settingCurFov;
        float step = (curFov - goalFov) / 4; 
        Animator a = weaponSlot.m_currentWeapon.gameObject.GetComponent<Animator>();
        a.SetBool("AimingOff", false);
        a.SetBool("AimingOn",true);
        for(int i = 0; i < 4; i++)
        {
            
            yield return null;
            curFov -= step;
            FirstCamera.fieldOfView = curFov;
        }
        FirstCamera.fieldOfView = goalFov;
        weaponSlot.Aim(true);
        canResume = true;
        coroutineRunning = false;
    }

    public void LateUpdate()
    {
        //Debug.Log(weaponSlot.currentWeapon.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(1).IsName("Idle"));
    }

    public void OnGUI()
    {
        
    }

    IEnumerator AimingOff()
    {
        coroutineRunning = true;
        weaponSlot.Aim(false);
        while (!canResume)
        {
            yield return null;
        }
        canResume = false;
        //all aiming animations are 4 frames.
        float curFov = settingCurFov;
        float step = (curFov - goalFov) / 4;
        Animator a = weaponSlot.m_currentWeapon.gameObject.GetComponent<Animator>();
        a.SetBool("AimingOn",false);
        a.SetBool("AimingOff",true);
        for (int i = 0; i < 4; i++)
        {

            yield return null;
            curFov += step;
            FirstCamera.fieldOfView = curFov;
        }
        FirstCamera.fieldOfView = settingCurFov;
        
        coroutineRunning = false;

    }

    // prevent problems after spamming right click. dont know why.
    private void ResetAiming()
    {
        
    }


    //callbacks.
    public void SetRecoil(float recoil)
    {
        battleUIBundle.AimChange(recoil);
    }

    public void SetAmmo(bool isShoot)
    {
        battleUIBundle.AmmoChange(isShoot);
    }

    public void RefreshUI(int maxAmmo, int curBackup, float baseAcc)
    {
        battleUIBundle.AmmoReset(maxAmmo, curBackup);
        battleUIBundle.ResetAim(baseAcc);
    }
}
