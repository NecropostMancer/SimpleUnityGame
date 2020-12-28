using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AmmoInspector : MonoBehaviour
{

    private Text curAmmo;
    private Text curMaga;
    private Text maxAmmo;

    // Start is called before the first frame update
    void Start()
    {
        curAmmo = transform.GetChild(0).GetComponent<Text>();
        curMaga = transform.GetChild(1).GetComponent<Text>();
        maxAmmo = transform.GetChild(2).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {

    }


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
