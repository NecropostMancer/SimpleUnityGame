using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BattleUIBundle : MonoBehaviour
{

    private Canvas UICanvasRoot;
    private GameObject ammoInspector;
    private AmmoInspector ammoInspectorRef; 
    private GameObject aim;
    private AimController aimRef;

    private int curAmmo;
    private int maxAmmo;
    private int maxMaga;
    


    // Start is called before the first frame update
    void Start()
    {
        UICanvasRoot = this.GetComponent<Canvas>();
        ammoInspector = this.transform.Find("ammoInspector").gameObject;
        ammoInspectorRef = ammoInspector.GetComponent<AmmoInspector>();
        aim = this.transform.Find("aim").gameObject;
        aimRef = aim.GetComponent<AimController>();
    }

    public void AimChange(float str)
    {
        aimRef.Expand(str);
        
    }
    public void ResetAim(float a)
    {
        aimRef.ResetAim(a);
    }
    public void AmmoReset(int maxAmmo,int maxBack)
    {
        curAmmo = this.maxAmmo = maxAmmo;
        this.maxMaga = maxBack;
        ammoInspectorRef.SetcurAmmo(curAmmo);
        ammoInspectorRef.SetcurMaga(maxMaga);
        ammoInspectorRef.SetmaxAmmo(maxAmmo);
    }
    public void AmmoChange(bool isShooting)
    {

        if (isShooting)
        {
            curAmmo--;
            ammoInspectorRef.SetcurAmmo(curAmmo);
        }
        else
        {
            curAmmo = maxAmmo;
            maxMaga--;
            ammoInspectorRef.SetcurAmmo(curAmmo);
            ammoInspectorRef.SetcurMaga(maxMaga);
        }
        
            
        
        
    }

}
