﻿using System.Collections;
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
    [SerializeField]

    private float curRecoil = 0.0f;
    [SerializeField]
    private float maxRecoilBeforeStop = 0.0f;
    [SerializeField]
    private float currentAcc = 1.0f;
    bool fire = false;
    bool isReloading = false;
    int totalShoot = 0;
    

    //debug
    public bool infAmmo = false;


    public enum FireMode { SINGLE, TRIPLE, AUTO }

    public FireMode fireMode = FireMode.SINGLE;

    private ProjectileGen shooter;

    private Transform shellThrowRelAt;
    private GameObject shell;
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

        shellThrowRelAt = transform.GetChild(3).transform;
        shell = transform.GetChild(4).gameObject;
        shell.SetActive(false);
    }

    // Update is called once per frame
    int sinceStop = 0;
    void Update()
    {
        if (curRecoil > 0.01)
        {
            if (fire == false)
            {
                sinceStop++;
                curRecoil -= recoveryStep;
                curRecoilVector +=  recoveryVector;

                float t = Mathf.SmoothStep(1, 0, sinceStop / (recovery * 60));

                curRecoil = maxRecoilBeforeStop * t ;
                curRecoilVector = maxRecoilVector * t;
                if (curRecoil < 0.01)
                {
                    curRecoilVector.x= curRecoilVector.y=0;
                    curRecoil = 0;
                    maxRecoilVector.x = maxRecoilVector.y = maxRecoilBeforeStop = 0;
                }
            }
        }
        else
        {
            sinceStop = 0;
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
                fire = false;
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
            StartCoroutine(ShootingTriggerCancelNextFrame());
            horizontalRecoilStr = Random.Range(-0.5f, 0.5f);
            verticalRecoilStr = Random.Range(0, 0.1f);
            if (!infAmmo)
            {
                currentAmmo--;
            }
            ApplyRecoil();
            center.setcurAmmo(currentAmmo);
            center.setcurMaga(currentBackup);
            ThrowShell();
        }
    }

    IEnumerator ShootingTriggerCancelNextFrame()
    {
        Animator a = GetComponent<Animator>();
        a.SetTrigger("Shoot");
        yield return null;
        a.ResetTrigger("Shoot");
    }

    public void LateUpdate()
    {
        Animator a = GetComponent<Animator>();
        a.ResetTrigger("Shoot");
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
    [SerializeField]
    private Vector2 curRecoilVector;
    [SerializeField]
    private Vector2 maxRecoilVector;
    private float recovery = 0.5f; // how many secs for full back; 
    [SerializeField]
    private float recoveryStep = 0.0f;
    [SerializeField]
    private Vector2 recoveryVector;
    public bool GetRecoilStr(out Quaternion res)
    {
        
        if(curRecoil < 0.005)
        {
            res = Quaternion.Euler(0,0,0);
            return false;
        }

        

        res = Quaternion.Euler(curRecoilVector.x, curRecoilVector.y, 0);

        return true;

        
    }

    private void ApplyRecoil()
    {
        curRecoil += recoil;
        maxRecoilBeforeStop = curRecoil;
        float vertical = -Mathf.Max((recoil + verticalRecoilStr), 0);

        curRecoilVector += new Vector2(vertical, horizontalRecoilStr * vertical);
        maxRecoilVector = curRecoilVector;
        //recoveryStep = maxRecoilBeforeStop / (recovery * 60);
        //recoveryVector = - curRecoilVector / (recovery * 60);
        
    }

    //todo: obj pool
    private void ThrowShell()
    {
        GameObject newShell = Instantiate(shell,shellThrowRelAt.position ,shell.transform.rotation);
        newShell.GetComponent<Rigidbody>().velocity = shell.transform.rotation * new Vector3(1, 0, 0);
        newShell.SetActive(true);
    }

}
