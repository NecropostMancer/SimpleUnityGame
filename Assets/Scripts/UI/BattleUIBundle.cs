using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BattleUIBundle : UIBundle
{

    private Canvas UICanvasRoot;
    private GameObject ammoInspector;
    private AmmoInspector ammoInspectorRef; 
    private GameObject aim;
    private AimController aimRef;

    private int curAmmo;
    private int maxAmmo;
    private int maxMaga;
    public override void SendCommand(UICommand command)
    {
        
        switch (command.GetType().Name)//oh.
        {
            case nameof(AmmoCommand):
                AmmoChange(command);
                break;
            case nameof(AimCommand):
                AimChange(command);
                break;
            default:
                break;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        UICanvasRoot = this.GetComponent<Canvas>();
        ammoInspector = this.transform.Find("ammoInspector").gameObject;
        ammoInspectorRef = ammoInspector.GetComponent<AmmoInspector>();
        aim = this.transform.Find("aim").gameObject;
        aimRef = aim.GetComponent<AimController>();
    }

    void AimChange(UICommand command)
    {
        AimCommand cmd = command as AimCommand;
        
        aimRef.Expand(cmd.str);
        
    }

    void AmmoChange(UICommand command)
    {
        AmmoCommand cmd = command as AmmoCommand;
        
        if (cmd.shot)
        {
            curAmmo--;
            ammoInspectorRef.SetcurAmmo(curAmmo);

        }else if (cmd.reload)
        {
            curAmmo = maxAmmo;
            maxMaga--;
            ammoInspectorRef.SetcurAmmo(curAmmo);
            ammoInspectorRef.SetcurMaga(maxMaga);
        }
        else if(cmd.reset)
        {
            curAmmo = maxAmmo = cmd.maxAmmo;
            maxMaga = cmd.maxMaga;
            ammoInspectorRef.SetcurAmmo(curAmmo);
            ammoInspectorRef.SetcurMaga(maxMaga);
            ammoInspectorRef.SetmaxAmmo(maxAmmo);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
