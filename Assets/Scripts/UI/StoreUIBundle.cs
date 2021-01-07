using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreUIBundle : UIBundle
{
    public GameObject m_scrollViewRoot;
    public GameObject m_fixedWeaponDataPanelRoot; //读入固定的宽，好安排可变部分的长度

    private RectTransform m_canvasTransform;
    public override void SendCommand(UICommand command)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_canvasTransform = GetComponent<RectTransform>();
        ReScale();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Screen.width);
        ReScale();
    }

    /*
     put it on a gameobject.
     public delegate void OnWindowResize();
 
public class WindowChange :  UIBehaviour{
 
    public static WindowChange instance = null;
    public OnWindowResize windowResizeEvent;
 
    void Awake() {
        instance = this;
    }
 
    protected override void OnRectTransformDimensionsChange ()
    {
        base.OnRectTransformDimensionsChange ();
        if(windowResizeEvent != null)
            windowResizeEvent();
    }
}
     
     */


    private void ReScale()
    {

        float resWidth = m_canvasTransform.rect.width;
        float resHeight = m_canvasTransform.rect.height;
        
        float leftWidth = resWidth - m_fixedWeaponDataPanelRoot.GetComponent<RectTransform>().rect.width;
        Rect rect = m_scrollViewRoot.GetComponent<RectTransform>().rect;
        rect.width = leftWidth - 80;
        m_scrollViewRoot.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rect.width);
        
    }
}
