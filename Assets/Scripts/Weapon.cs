using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    [SerializeField]
    private int magazineSize;
    private int currentAmmo;
    [SerializeField]
    private int backupMagazine;
    private int currentBackup;
    [SerializeField]
    private int shootCd = 5;
    private int currentCd;
    [SerializeField]
    private float damageMult = 1.0f;
    [SerializeField]
    private float accuracy = 0.9f;
    //如果要支持后座，需要想办法控制到相机。
    [SerializeField]
    private float recoil = 10.0f;
    private float curRecoil = 0.0f;
    private float currentAcc = 1.0f;
    bool fire = false;
    bool isReloading = false;
    int totalShoot = 0;

    public enum FireMode { SINGLE, TRIPLE, AUTO }

    public FireMode fireMode = FireMode.SINGLE;

    private ProjectileGen shooter;

    UICenter center;
    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = magazineSize;
        currentBackup = backupMagazine;
        currentAcc = accuracy;
        currentCd = shootCd;
        shooter = transform.GetChild(1).GetComponent<ProjectileGen>();
        shooter.setDamageBuff(damageMult);
        shooter.setReady(true);

        center = GameObject.Find("SomeGameObject").GetComponent<UICenter>();

        //center = UICenter.GetCenter();
        center.setcurAmmo(currentAmmo);
        center.setcurMaga(currentBackup);
        center.setmaxAmmo(magazineSize);
    }

    // Update is called once per frame
    void Update()
    {
        if (curRecoil > 0.01)
        {
            curRecoil *= 0.85f;
            if (curRecoil < 0)
            {
                curRecoil = 0;
            }
        }
    }

    private void FixedUpdate()
    {
       
        if (currentCd != shootCd)
        {
            currentCd++;
        }
        if (isReloading)
        {
            return;
        }
        if (fire)
        {
            if(currentAmmo == 0)
            {
                reload();
                return;
            }
            
               
            
            if (currentCd != shootCd)
            {
                if (fireMode != FireMode.AUTO)
                {
                    fire = false;
                }
                return;

            }
            if (fireMode == FireMode.SINGLE)
            {
                fire = false;
                currentCd = 0;
            }
            else if (fireMode == FireMode.TRIPLE)
            {
                //Debug.Log(totalShoot);
                totalShoot++;
                if (totalShoot == 3)
                {
                    totalShoot = 0;
                    fire = false;
                    currentCd = 0;
                }
                fire = true;
            }
            else if (fireMode == FireMode.AUTO)
            {
                currentCd = 0;
            }

            shooter.shootProjectile();
            horizontalRecoilStr = Random.Range(-0.5f, 0.5f);
            verticalRecoilStr = Random.Range(0, 0.1f);
            currentAmmo--;
            curRecoil += recoil;
            center.setcurAmmo(currentAmmo);
            center.setcurMaga(currentBackup);
            
        }
    }

    public void reload()
    {
        if(currentBackup == 0)
        {
            return;
        }
        isReloading = true;
        GetComponent<Animator>().SetTrigger("Reload");
        
    }

    public void ReloadingAnimationDone()
    {
        if (currentBackup != 0)
        {
            currentAmmo = magazineSize;
            currentBackup--;
            center.setcurAmmo(currentAmmo);
            center.setcurMaga(currentBackup);
        }
        isReloading = false;
        
    }

    public void ReloadingSoundPlay()
    {
        GetComponent<Animator>().ResetTrigger("Reload"); //prevent potential animation redundent
        //play sound
    }

    public void Fire(bool a)
    {
        fire = a;
        
    }

    private float horizontalRecoilStr;
    private float verticalRecoilStr;
    public bool GetRecoilStr(out Quaternion res)
    {
        if(curRecoil < 0.005)
        {
            res = Quaternion.Euler(0,0,0);
            return false;
        }

        
        float vertical = - Mathf.Max((curRecoil + verticalRecoilStr),0);

        res = Quaternion.Euler(vertical, horizontalRecoilStr*vertical, 0);

        return true;

        
    }

}
