using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class GameUIManager : Singleton<GameUIManager>
{
    //????
    WeakReference<UIBundle> bundle;

    //操控当前的UIBundle，一个场景只能有一个Bundle，(必须是canvas)
    //编辑时保证只有一个Bundle存在于一个Scene中，
    //运行时依赖于GameSceneManager的正确行为顺序。

    bool isSceneSwitching = false;
    
    public void Inform(UICommand cmd)
    {
        bundle.TryGetTarget(out UIBundle b);
        if (b)
        {
            b.SendCommand(cmd);
        }
    }

    public void InvokeSceneManager(int type)
    {
        
        if (bundle != null)
        {
            HideAll();
        }
    }
    
    public void InstallUI(UIBundle UIbundle)
    {
        
        bundle = new WeakReference<UIBundle>(UIbundle);
        bundle.TryGetTarget(out UIBundle b);
        if (b)
        {
            
        }
    }
    public void RemoveUI()
    {
        bundle = null;
    }
    public void HideAll()
    {
        bundle.TryGetTarget(out UIBundle b);
        if (b)
        {
            b.gameObject.GetComponent<Canvas>().enabled = false;
            //b.gameObject.SetActive(false);
        }
    }

    public void ShowAll()
    {
        bundle.TryGetTarget(out UIBundle b);
        if (b)
        {
            b.gameObject.GetComponent<Canvas>().enabled = true;
            //b.gameObject.SetActive(true);
        }
    }

    private GameObject m_ActiveDialog;
    private Button m_ActiveOK;
    private Button m_ActiveCancel;
    public bool CallMsgbox(string text = "PAUSE", MsgCallback whenOk = null, string okText = "OK")
    {
        if (GameAssetsManager.instance.IsBusy()) return false;
        bundle.TryGetTarget(out UIBundle b);
        m_ActiveDialog = Instantiate(m_MsgboxCache,b.transform) as GameObject;
        TextMeshProUGUI[] texts = new TextMeshProUGUI[2];
        texts = m_ActiveDialog.GetComponentsInChildren<TextMeshProUGUI>();
        texts[0].text = text;
        texts[1].text = okText;
        m_ActiveOK = m_ActiveDialog.GetComponentInChildren<Button>();
        m_ActiveOK.onClick.AddListener(delegate
        {
            
            Destroy(m_ActiveDialog);
            m_ActiveDialog = null;
            m_ActiveOK = null;
            if (whenOk !=null)
            {
                whenOk();
            }
        });
        return true;
    }

    public bool CallDialog(string text, MsgCallback whenOk,MsgCallback whenCancel = null)
    {
        if (GameAssetsManager.instance.IsBusy()) return false;
        bundle.TryGetTarget(out UIBundle b);
        m_ActiveDialog = Instantiate(m_DialogCache, b.transform) as GameObject;
        m_ActiveDialog.GetComponentInChildren<TextMeshProUGUI>().text = text;
        Button []buttons = m_ActiveDialog.GetComponentsInChildren<Button>();
        m_ActiveOK = buttons[0];
        m_ActiveOK.onClick.AddListener(delegate
        {

            Destroy(m_ActiveDialog);
            m_ActiveDialog = null;
            m_ActiveOK = null;
            m_ActiveCancel = null;
            whenOk();
        });
        m_ActiveCancel = buttons[1];
        m_ActiveCancel.onClick.AddListener(delegate
        {

            Destroy(m_ActiveDialog);
            m_ActiveDialog = null;
            m_ActiveOK = null;
            m_ActiveCancel = null;
            whenCancel();
        });
        return true;
    }

    public void ClickOK()
    {
        if(m_ActiveOK != null)
        {
            m_ActiveOK.onClick.Invoke();
        }
    }

    public void ClickCancel()
    {
        if(m_ActiveCancel != null)
        {
            m_ActiveCancel.onClick.Invoke();
        }
    }

    public delegate void MsgCallback();

    private UnityEngine.Object m_DialogCache;
    private UnityEngine.Object m_MsgboxCache;
    // Start is called before the first frame update
    void Start()
    {
        m_DialogCache = Resources.Load("UI/Dialog");
        m_MsgboxCache = Resources.Load("UI/Msgbox");
    }

    // Update is called once per frame
    void Update()
    {
        //GameObject.FindGameObjectWithTag("UIBundle");
    }

}
