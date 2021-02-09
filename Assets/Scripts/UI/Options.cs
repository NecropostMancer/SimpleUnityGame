using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField]
    private Slider[] m_VolumeGroup = new Slider[4];
    [SerializeField]
    private Slider m_MouseSensitivity;

    private GameSetting m_GameSetting;
    // Start is called before the first frame update
    void Start()
    {
        m_GameSetting = GameAssetsManager.instance.GetSetting();



        m_VolumeGroup[0].onValueChanged.AddListener(AudioManager.instance.SetMainVolume);
        m_VolumeGroup[1].onValueChanged.AddListener(AudioManager.instance.SetGameSFXVolume);
        m_VolumeGroup[2].onValueChanged.AddListener(AudioManager.instance.SetUISFXVolume);
        m_VolumeGroup[3].onValueChanged.AddListener(AudioManager.instance.SetBGMVolume);


        m_VolumeGroup[0].value = m_GameSetting.volume[0];
        m_VolumeGroup[1].value = m_GameSetting.volume[1];
        m_VolumeGroup[2].value = m_GameSetting.volume[2];
        m_VolumeGroup[3].value = m_GameSetting.volume[3];

        m_MouseSensitivity.onValueChanged.AddListener(delegate (float v) {
            m_GameSetting.sen = v;
        });

        m_MouseSensitivity.value = m_GameSetting.sen;
    }


    private void OnDisable()
    {
        m_GameSetting.volume[0] = m_VolumeGroup[0].value;
        m_GameSetting.volume[1] = m_VolumeGroup[1].value;
        m_GameSetting.volume[2] = m_VolumeGroup[2].value;
        m_GameSetting.volume[3] = m_VolumeGroup[3].value;


        GameAssetsManager.instance.UpdateSetting();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
