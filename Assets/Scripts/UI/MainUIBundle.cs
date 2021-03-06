﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIBundle : UIBundle
{

    public AudioClip backgroundMusic;
    [SerializeField]
    private RectTransform m_Setting;
    
    void LoadSceneAndClear()
    {
        GameAssetsManager.instance.LoadSceneByName("LevelSelection");
        GameAssetsManager.instance.NewSave();
    }

    void LoadSaveAndScene()
    {
        GameAssetsManager.instance.LoadSave();
        GameAssetsManager.instance.LoadSceneByName("LevelSelection");
    }
    void OpenSetting()
    {
        m_Setting.gameObject.SetActive(true);
    }
    void CloseSetting()
    {
        m_Setting.gameObject.SetActive(false);
        
    }
    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }

    // 依赖于prefab的层级结构。
    // Start is called before the first frame update
    void Start()
    {
        Transform y = transform.GetChild(0).GetChild(0);
        y.Find("LOAD").GetComponent<Button>().onClick.AddListener(LoadSaveAndScene);
        if (!GameAssetsManager.instance.IsSaveVaild())
        {
            y.Find("LOAD").gameObject.SetActive(false);
        }
        y.Find("NEW").GetComponent<Button>().onClick.AddListener(LoadSceneAndClear);
        y.Find("QUIT").GetComponent<Button>().onClick.AddListener(QuitGame);
        y.Find("OPTION").GetComponent<Button>().onClick.AddListener(OpenSetting);
        m_Setting.transform.GetComponentInChildren<Button>().onClick.AddListener(CloseSetting);
        AudioManager.instance.PlayBGM(backgroundMusic);

        var panel = transform.GetChild(0).GetComponent<Image>();

        m_Setting.gameObject.SetActive(false);

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void SendCommand(UICommand command)
    {
        throw new System.NotImplementedException();
    }
}
