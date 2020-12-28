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
    private aimController aimRef;

    private int curAmmo;
    private int maxAmmo;
    private int maxMaga;
    public override void SendCommand(UICommand command)
    {
        
        switch (command.GetType().Name)//oh.
        {
            case nameof(AmmoCommand):
                ammoChange(command);
                break;
            case nameof(AimCommand):
                aimChange(command);
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
        aimRef = aim.GetComponent<aimController>();
    }

    void aimChange(UICommand command)
    {
        AimCommand cmd = command as AimCommand;
        
        aimRef.expand(cmd.str);
        
    }

    void ammoChange(UICommand command)
    {
        AmmoCommand cmd = command as AmmoCommand;
        
        if (cmd.shot)
        {
            curAmmo--;
            ammoInspectorRef.setcurAmmo(curAmmo);

        }else if (cmd.reload)
        {
            curAmmo = maxAmmo;
            maxMaga--;
            ammoInspectorRef.setcurAmmo(curAmmo);
            ammoInspectorRef.setcurMaga(maxMaga);
        }
        else if(cmd.reset)
        {
            curAmmo = maxAmmo = cmd.maxAmmo;
            maxMaga = cmd.maxMaga;
            ammoInspectorRef.setcurAmmo(curAmmo);
            ammoInspectorRef.setcurMaga(maxMaga);
            ammoInspectorRef.setmaxAmmo(maxAmmo);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
