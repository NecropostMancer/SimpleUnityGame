using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    public static LevelEnd gm_CurrentInstance;

    public AudioClip m_Victory;
    public AudioClip m_Failure;

    private int m_Money;
    [SerializeField]
    private GameObject m_MoneyRoot;
    [SerializeField]
    private TextMeshProUGUI m_MoneyText;
    private int m_Time;
    [SerializeField]
    private GameObject m_TimeRoot;
    [SerializeField]
    private TextMeshProUGUI m_TimeText;
    private int m_Kill;

    [SerializeField]
    private GameObject m_TitleRoot;
    [SerializeField]
    private Button m_Continue;

    public bool g_Animate;
    // Start is called before the first frame update
    void Start()
    {
        
        gm_CurrentInstance = this;
        m_TitleRoot.SetActive(false);
        m_MoneyRoot.SetActive(false);
        m_TimeRoot.SetActive(false);
        m_Continue.onClick.AddListener(delegate {
            BattleManager.instance.QuitBattle();
            GameAssetsManager.instance.GetSave().money += m_Money;
            
            GameAssetsManager.instance.LoadSceneByName("LevelSelection");
            SceneManager.UnloadSceneAsync("EndLevelAddtive");
            gm_CurrentInstance = null;
        });
        if (BattleManager.instance.isWin())
        {
            Victory();
        }
        else
        {
            Failure();
        }
    }
    private int frame = 0;
    // Update is called once per frame
    void Update()
    {
        if (!g_Animate) return;
        if(frame == 0)
        {
            m_TitleRoot.SetActive(true);
            m_TitleRoot.GetComponent<ThrowIn>().g_Start = true;
        }else
        if(frame == 90)
        {
            m_MoneyRoot.SetActive(true);
            m_MoneyRoot.GetComponent<ThrowIn>().g_Start = true;
        }
        else
        if(frame == 180)
        {
            m_TimeRoot.SetActive(true);
            m_TimeRoot.GetComponent<ThrowIn>().g_Start = true;
        }
        
        frame++;
    }

    public void Victory()
    {
        m_TitleRoot.GetComponent<TextMeshProUGUI>().text = "VICTORY";
        m_Money = 1000;
        m_MoneyText.text = "1000";
        float time = BattleManager.instance.GetTimePassed();
        m_TimeText.text = Mathf.FloorToInt(time / 60).ToString() + ":" + Mathf.FloorToInt(time % 60).ToString("00");
        AudioManager.instance.PlayLongSound(m_Victory);
        g_Animate = true;
    }

    public void Failure()
    {
        m_TitleRoot.GetComponent<TextMeshProUGUI>().text = "FAIL";
        m_Money = 250;
        m_MoneyText.text = "250";
        float time = BattleManager.instance.GetTimePassed();
        m_TimeText.text = Mathf.FloorToInt(time / 60).ToString() + ":" + Mathf.FloorToInt(time % 60).ToString("00");
        g_Animate = true;
    }
}
