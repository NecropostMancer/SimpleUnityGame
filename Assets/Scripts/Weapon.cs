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
    private float currentAcc = 1.0f;
    bool fire = false;
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
        if (Input.GetMouseButtonDown(0))
        {
            fire = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            fire = false;
        }
        if (Input.GetMouseButtonDown(2))
        {
            reload();
        }
    }

    private void FixedUpdate()
    {

        if (currentCd != shootCd)
        {
            currentCd++;
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
            currentAmmo--;
            center.setcurAmmo(currentAmmo);
            center.setcurMaga(currentBackup);
            
        }
    }

    void reload()
    {
        if (currentBackup != 0)
        {
            currentAmmo = magazineSize;
            currentBackup--;
            center.setcurAmmo(currentAmmo);
            center.setcurMaga(currentBackup);
        }
    }

}
