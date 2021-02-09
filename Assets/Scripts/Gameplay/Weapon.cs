using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class Weapon : Playable
{
    [SerializeField]
    private int m_MagazineSize;
    private int m_CurrentAmmo;
    [SerializeField]
    private int m_BackupMagazine;
    private int m_CurrentBackup;
    [SerializeField]
    private float m_ShootCd = 5;
    private float m_CurrentCd;
    [SerializeField]
    private float m_DamageMult = 1.0f;
    [SerializeField]
    [Range(0.0f,1.0f)]
    private float m_Accuracy = 0.9f;
    //如果要支持后座，需要想办法控制到相机。
    [SerializeField]
    private float m_Recoil = 10.0f;
    private float m_RecoilBackup;
    [SerializeField]

    private float m_CurRecoil = 0.0f;
    [SerializeField]
    private float m_MaxRecoilBeforeStop = 0.0f;
    [SerializeField]
    private float m_CurrentAcc = 1.0f;
    private bool m_Fire = false;
    private bool m_IsReloading = false;
    private int m_TotalShoot = 0;

    public bool m_IsAiming = false;

    //debug
    public bool m_InfAmmo = false;


    public enum FireMode { SINGLE, TRIPLE, AUTO }

    [SerializeField]
    private FireMode m_FireMode = FireMode.SINGLE;
    [SerializeField]
    private bool m_ModeSwitchable = false;
    private ProjectileGen m_Shooter;

    private bool m_ThrowShell;
    private Transform m_ShellThrowRelAt;
    private GameObject m_Shell;

    //private readonly AmmoCommand command = new AmmoCommand();
    //private readonly AimCommand aimCommand = new AimCommand();

    private float m_HorizontalRecoilStr;
    private float m_VerticalRecoilStr;
    [SerializeField]
    private Vector2 m_CurRecoilVector;
    [SerializeField]
    private Vector2 m_MaxRecoilVector;
    private readonly float m_Recovery = 0.5f; // how many secs for full back; 
    [SerializeField]
    private float m_RecoveryStep = 0.0f;
    [SerializeField]
    private Vector2 m_RecoveryVector;
    [SerializeField]
    private AudioClip[] m_FireSound;
    private AudioClip[] m_ReloadSound;
    private AudioSource m_AudioSource;

    private Animator m_Animator;
    private bool m_IsPulled;
    private static PlayerController sm_PlayerController; 

    // 依赖于prefab的层级结构。
    // Start is called before the first frame update
    void Start()
    {
        m_CurrentAmmo = m_MagazineSize;
        m_CurrentBackup = m_BackupMagazine;
        m_CurrentAcc = m_Accuracy;
        m_CurrentCd = m_ShootCd;
        m_Shooter = transform.GetChild(1).GetComponent<ProjectileGen>();
        m_Shooter.SetDamageBuff(m_DamageMult);
        m_RecoilBackup = m_Recoil;
        m_Animator = GetComponent<Animator>();
        m_Shooter.SetReady(true);
        m_IsPulled = false;
        sm_PlayerController = GetComponentInParent<PlayerController>();
        m_AudioSource = GetComponent<AudioSource>();
        if (m_UsingLoopingFireSound)
        {
            m_AudioSource.loop = true;
        }
        AudioManager.instance.AudioRegister(m_AudioSource, AudioManager.AudioType.GameSFX);
        if (sm_PlayerController == null)
        {
            Debug.LogError("Weapon: No controller can be found.");
        }
        else
        {
            
            sm_PlayerController.RefreshUI(m_MagazineSize, m_CurrentBackup, m_CurrentAcc,m_FireMode,m_CurrentAmmo);
        }
        if (transform.childCount > 4)
        {
            m_ThrowShell = true;
            m_ShellThrowRelAt = transform.GetChild(3).transform;
            m_Shell = transform.GetChild(4).gameObject;
            m_Shell.SetActive(false);
        }
        else
        {
            m_ThrowShell = false;
        }
        if (m_FireMode == FireMode.TRIPLE)
        {
            if (m_FireSound.Length != 0)
            {
                m_FireSound[0] = Utils.MakeSubclip(m_FireSound[0], 1.300f, 2.000f);
            }
        }
        
    }

    private void OnEnable()
    {
        if (sm_PlayerController != null)
        {
            sm_PlayerController.RefreshUI(m_MagazineSize, m_CurrentBackup, m_CurrentAcc,m_FireMode,m_CurrentAmmo);
        }
        
    }

    // Update is called once per frame
    int sinceStop = 0;
    void Update()
    {
        if (m_CurRecoil > 0.01)
        {
            if (!m_Fire)
            {
                sinceStop++;
                m_CurRecoil -= m_RecoveryStep;
                m_CurRecoilVector +=  m_RecoveryVector;

                float t = Mathf.SmoothStep(1, 0, sinceStop / (m_Recovery * 60));

                m_CurRecoil = m_MaxRecoilBeforeStop * t ;
                m_CurRecoilVector = m_MaxRecoilVector * t;
                if (m_CurRecoil < 0.01)
                {
                    m_CurRecoilVector.x= m_CurRecoilVector.y=0;
                    m_CurRecoil = 0;
                    m_MaxRecoilVector.x = m_MaxRecoilVector.y = m_MaxRecoilBeforeStop = 0;
                    //aimCommand.str = Mathf.Clamp((1f - accuracy) * 10, 1, 10);
                    sm_PlayerController.SetRecoil(0);
                }
            }
            //aimCommand.str = Mathf.Clamp((1f - accuracy) * 10 + curRecoil / (curRecoil + 20) * 10,1,10);
            //SendUICommand(aimCommand);
            sm_PlayerController.SetRecoil(m_CurRecoil);
        }
        else
        {
            sinceStop = 0;

        }

        if (m_CurrentCd < m_ShootCd)
        {
            m_CurrentCd+= Time.deltaTime;
        }
        if (m_IsReloading || !m_IsPulled)
        {
            return;
        }
        if (m_Fire)
        {
            if (m_CurrentAmmo == 0)
            {
                Reload();
                m_Fire = false;
                ShootingStateExit();
                return;
            }



            if (m_CurrentCd < m_ShootCd)
            {
                if (m_FireMode != FireMode.AUTO)
                {
                    m_Fire = false;
                    ShootingStateExit();
                }
                return;

            }
            if (m_FireMode == FireMode.SINGLE)
            {
                m_Fire = false;
                ShootingStateExit();
                m_CurrentCd = 0;
            }
            else if (m_FireMode == FireMode.TRIPLE)
            {
                //Debug.Log(totalShoot);

                m_TotalShoot++;

                if (m_TotalShoot == 3)
                {
                    m_TotalShoot = 0;
                    m_Fire = false;
                    ShootingStateExit();
                    m_CurrentCd = 0;
                }
                m_Fire = true;
            }
            else if (m_FireMode == FireMode.AUTO)
            {
                m_CurrentCd = 0;
                shot = true;
            }

            m_Shooter.ShootProjectile(m_IsAiming ? 1f : m_Accuracy / ((m_CurRecoil + 30) / 30)); //目前和屏幕上的准星大小不匹配。
            StartCoroutine(ShootingTriggerCancelNextFrame());
            m_HorizontalRecoilStr = Random.Range(-0.5f, 0.5f);
            m_VerticalRecoilStr = Random.Range(0, 0.1f);
            PlayShotSound();
            if (!m_InfAmmo)
            {
                m_CurrentAmmo--;
                //command.shot = true;
                sm_PlayerController.SetAmmo(true);
                //command.shot = false;
            }
            ApplyRecoil();

            ThrowShell();
        }
        if (shot)
        {
            if (m_Fire)
            {
                return;
            }
            shot = false;
            ShootingStateExit();
        }

    }
    bool shot = false;
    int triple = 0;
    

    IEnumerator ShootingTriggerCancelNextFrame()
    {
        
        m_Animator.SetTrigger("Shoot");

        yield return null;
        m_Animator.ResetTrigger("Shoot");
    }
    [SerializeField]
    private bool m_UsingLoopingFireSound = false;
    private void ShootingStateExit()
    {
        
        m_Animator.SetTrigger("ShootStop");
        if(m_UsingLoopingFireSound)
        {
            m_AudioSource.Stop();
        }
    }

    public void LateUpdate()
    {
        
        m_Animator.ResetTrigger("Shoot");
    }

    public void Reload()
    {
        if(m_CurrentBackup == 0)
        {
            return;
        }
        m_IsReloading = true;
        m_Animator.SetTrigger("Reload");
        
    }

    //animation event callback
    public void ReloadingAnimationDone()
    {
        if (m_CurrentBackup != 0)
        {
            m_CurrentAmmo = m_MagazineSize;
            m_CurrentBackup--;
            //command.reload = true;
            //SendUICommand(command);
            //command.reload = false;
            sm_PlayerController.SetAmmo(false);
        }
        m_IsReloading = false;
        
    }

    public void PullAnimationDone()
    {
        m_IsPulled = true;
    }


    public void ReloadingSoundPlay()
    {
        m_Animator.ResetTrigger("Reload"); //prevent potential animation redundent
        //play sound here
    }
    public void PlayShotSound()
    {
        if (m_FireSound.Length != 0)
        {
            m_AudioSource.clip = m_FireSound[Random.Range(0, m_FireSound.Length)];
            m_AudioSource.Play();
            //AudioManager.instance.PlayOneSound(m_FireSound[Random.Range(0, m_FireSound.Length)],transform.position);
        }
    }

    public void Fire(bool a)
    {
        m_Fire = a;
        
    }

    
    public bool GetRecoilStr(out Quaternion res)
    {
        
        if(m_CurRecoil < 0.005)
        {
            res = Quaternion.Euler(0,0,0);
            return false;
        }

        

        res = Quaternion.Euler(m_CurRecoilVector.x, m_CurRecoilVector.y, 0);

        return true;

        
    }
    private float RecoilApply;
    

    private void ApplyRecoil()
    {
        RecoilApply = (0.9f + 1f / (18.5f + m_CurRecoil))* m_Recoil;
        if(m_CurRecoil > 80)
        {
            RecoilApply /= 8;
        }
        m_CurRecoil += RecoilApply;
        m_MaxRecoilBeforeStop = RecoilApply;
        float vertical = -Mathf.Max((RecoilApply + m_VerticalRecoilStr), 0);

        m_CurRecoilVector += new Vector2(vertical, m_HorizontalRecoilStr * vertical);
        m_MaxRecoilVector = m_CurRecoilVector;
        //recoveryStep = maxRecoilBeforeStop / (recovery * 60);
        //recoveryVector = - curRecoilVector / (recovery * 60);
        
    }

    //todo: obj pool
    private void ThrowShell()
    {
        if (!m_ThrowShell)
        {
            return;
        }
        var rand = Quaternion.Euler(Random.Range(-20.0f, 20.0f), 0, 0);
        GameObject newShell = Instantiate(m_Shell,m_ShellThrowRelAt.position ,m_Shell.transform.rotation * rand);
        newShell.GetComponent<Rigidbody>().velocity = m_Shell.transform.rotation * new Vector3(Random.Range(0.6f,1.1f), 0, Random.Range(-0.6f, -1.1f));
        newShell.SetActive(true);
    }
    public WeaponDescriptor getDescriptor()
    {
        WeaponDescriptor weapon = new WeaponDescriptor();
        weapon.acc = m_CurrentAcc;
        weapon.ammo = m_MagazineSize;
        weapon.damage = m_DamageMult;
        weapon.fireRate = Mathf.Ceil(1 / m_ShootCd);
        weapon.fireMode = (int)m_FireMode;
        weapon.name = gameObject.name;
        return weapon;
    }

    public void Aim(bool state)
    {
        if (state)
        {
            m_Recoil /= 3;
            m_IsAiming = true;
        }
        else
        {
            m_Recoil = m_RecoilBackup;
            m_IsAiming = false;
        }
    }

    public void SwitchFireMode()
    {
        if (m_ModeSwitchable)
        {
            if(m_FireMode == FireMode.AUTO)
            {
                m_FireMode = FireMode.SINGLE;
            }
            else
            {
                m_FireMode = FireMode.AUTO;
            }
            sm_PlayerController.RefreshUI(m_MagazineSize, m_CurrentBackup, m_CurrentAcc, m_FireMode, m_CurrentAmmo);
        }
    }

    public void ClearAnimator()
    {
        m_Animator.Rebind();
        m_Animator.Update(0f);
        if (m_IsReloading)
        {
            m_IsReloading = false;
        }
        m_IsPulled = false;
        m_IsAiming = false;
    }
}
