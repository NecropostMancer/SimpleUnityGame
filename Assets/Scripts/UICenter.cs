using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 暂时提供显示弹药的功能
 
 */
public class UICenter : MonoBehaviour
{
    public Text curAmmo;
    public Text curMaga;
    public Text maxAmmo;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static UICenter center;

    

    public void setcurAmmo(int a)
    {
        curAmmo.text = a.ToString();
    }

    public void setcurMaga(int a)
    {
        curMaga.text = a.ToString();
    }

    public void setmaxAmmo(int a)
    {
        maxAmmo.text = a.ToString();
    }
}
