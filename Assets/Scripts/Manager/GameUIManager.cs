using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameUIManager : Singleton<GameUIManager>
{
    //????
    WeakReference<UIBundle> bundle;

    GameAssetsManager gameSceneManager; //尽量让同一级之间互通.

    //操控当前的UIBundle，一个场景只能有一个Bundle，
    //编辑时保证只有一个Bundle存在于一个Scene中，
    //运行时依赖于GameSceneManager的正确行为顺序。
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
        if(type == 0)
        {
            gameSceneManager.LoadMainMenu(Callback);
        }
        else if(type == -1)
        {
            gameSceneManager.LoadSelection(Callback);
        }
        else if(type == -2)
        {
            gameSceneManager.LoadStore(Callback);
        }
        else
        {
            gameSceneManager.LoadBattleScene(type,Callback);
        }
        if (bundle != null)
        {
            HideAll();
        }
    }
    //场景被加载完成后，貌似还得等一帧物体才能有反应，因此等待一下再调这个回调。
    //用来加载当前场景的UI(主要就是在当前场景里面找放好的UIRoot)，找到后做些杂活，
    //目前只是显示UI。
    private void Callback()
    {
        StartCoroutine(DelayCallback());
    }

    IEnumerator DelayCallback()
    {
        yield return new WaitForSeconds(0.1f);
        InstallUI();
        ShowAll();
    }

    public void InstallUI()
    {
        GameObject go = GameObject.FindGameObjectWithTag("UIBundle");
        bundle = new WeakReference<UIBundle>(go.GetComponent<UIBundle>());
        bundle.TryGetTarget(out UIBundle b);
        if (b)
        {
            b.InjectManager(this);
        }
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

    // Start is called before the first frame update
    void Start()
    {
        gameSceneManager = GameAssetsManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        //GameObject.FindGameObjectWithTag("UIBundle");
    }
}
