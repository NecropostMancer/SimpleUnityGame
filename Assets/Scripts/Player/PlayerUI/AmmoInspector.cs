using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AmmoInspector : MonoBehaviour
{

    private Text curAmmo;
    private Text curMaga;
    private Text maxAmmo;

    private bool enableExtraDigit = false;
    // Start is called before the first frame update
    // 依赖于prefab的层级结构。
    void Start()
    {
        curAmmo = transform.GetChild(1).GetComponent<Text>();
        curMaga = transform.GetChild(2).GetComponent<Text>();
        maxAmmo = transform.GetChild(3).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void SetcurAmmo(int a)
    {
        if (enableExtraDigit)
        {
            curAmmo.text = a.ToString("000");
        }
        else
        {
            curAmmo.text = a.ToString("00");
        }
    }

    public void SetcurMaga(int a)
    {
        curMaga.text = a.ToString();
    }

    public void SetmaxAmmo(int a)
    {
        if (a > 99)
        {
            enableExtraDigit = true;
            maxAmmo.text = a.ToString("000");
        }
        else
        {
            enableExtraDigit = false;
            maxAmmo.text = a.ToString("00");
        }
    }
}
