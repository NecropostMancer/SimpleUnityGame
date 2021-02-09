using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSelectionUI : UIBundle
{
    public LevelSelectionCamera m_SceneCamera;
    
    public GameObject m_FoldingPanel;
    public TextMeshProUGUI m_IntroText;
    public Button m_GoStoreButton;
    public Button m_GoLevelButton;
    public Button m_GoQuitButton;
    public GameObject m_Arrow;
    public GameObject m_FirstTime;
    
    public GameObject m_ImageGroup;
    public Image m_ImageGroupContent;
    public TextMeshProUGUI m_LevelName;
    public TextMeshProUGUI m_LevelDiffculty;
    public TextMeshProUGUI m_MoneyText;
    public TextMeshProUGUI m_ModeText;
    public TextMeshProUGUI m_GoalText;
    public Button m_Easy;
    public Button m_Medium;
    public Button m_Hard;
    
    private int m_CurSelected;
    private LevelPivot m_CurSelectedPivot;
    private float m_Difficulty = 1;

    private ProfileData m_ProfileDataRef;
    [SerializeField]
    private AudioClip BGM;
    [SerializeField]
    private AudioClip m_Start;
    [SerializeField]
    private AudioClip m_Click;
    private AudioSource m_AudioSource;
    void Start()
    {
        m_GoStoreButton.onClick.AddListener(GoStore);
        m_GoLevelButton.onClick.AddListener(GoLevel);
        m_GoQuitButton.onClick.AddListener(Quit);
        m_Easy.onClick.AddListener(delegate { SelectDiffculty(0); });
        m_Medium.onClick.AddListener(delegate { SelectDiffculty(1); });
        m_Hard.onClick.AddListener(delegate { SelectDiffculty(2); });
        m_ProfileDataRef = GameAssetsManager.instance.GetSave();
        m_MoneyText.text = "$:" + m_ProfileDataRef.money;
        m_PanelOpenPos = m_FoldingPanel.GetComponent<RectTransform>().anchoredPosition;

        m_IntroTextRect = m_IntroText.GetComponent<RectTransform>();
        m_FoldingPanelRect = m_FoldingPanel.GetComponent<RectTransform>();

        m_Easy.interactable = false;
        m_Medium.interactable = true;
        m_Hard.interactable = true;

        AudioManager.instance.PlayBGM(BGM);

        m_AudioSource = GetComponent<AudioSource>();

        GameUIManager.instance.InstallUI(this);
        
        //Hide();
    }
    private void GoStore()
    {
        GameAssetsManager.instance.LoadSceneByName("Store");
    }
    private void GoLevel()
    {
        if (m_CurSelectedPivot != null)
        {
            GameAssetsManager.instance.LoadSceneByName("BattleScene" + m_CurSelected.ToString(),null,(int)m_CurSelectedPivot.g_Mode,(int)(m_CurSelectedPivot.g_Req * m_Difficulty));
            if (m_CurSelectedPivot.g_Track != null && m_CurSelectedPivot.g_Track.Length > 0)
            {
                AudioManager.instance.PlayBGM(m_CurSelectedPivot.g_Track[Random.Range(0,m_CurSelectedPivot.g_Track.Length)]);
            }
            m_AudioSource.clip = m_Start;
            m_AudioSource.Play();
        }
    }
    private void Quit()
    {
        GameAssetsManager.instance.LoadSceneByName("MainMenu");
    }

    // Update is called once per frame
    void Update()
    {
        m_MoneyText.text = "$:" + m_ProfileDataRef.money.ToString();
        
        Vector3 arrowDir = new Vector3(m_SceneCamera.transform.position.x, m_Arrow.transform.position.y, m_SceneCamera.transform.position.z);
        m_Arrow.transform.LookAt(arrowDir);
        m_FirstTime.transform.LookAt(arrowDir);
        m_FirstTime.transform.Rotate(0, 180, 0);
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            if (results.Count != 0)
            {
                bool isUI = false;
                foreach (RaycastResult hit in results)
                {
                    if (hit.gameObject.GetComponent<Image>() != null)
                    {
                        isUI = true;
                        break;
                    }
                }
                if (!isUI)
                {
                    foreach (RaycastResult hit in results)
                    {
                        LevelPivot levelPivot = hit.gameObject.GetComponent<LevelPivot>();
                        if (levelPivot)
                        {
                            if (m_FirstTime.activeSelf)
                            {
                                m_FirstTime.SetActive(false);
                            }
                            SelectLevel(levelPivot);
                            m_AudioSource.clip = m_Click;
                            m_AudioSource.Play();
                            return;
                        }

                    }
                    Hide();
                }


            }
        }
    }
    
    public void Hide()
    {
        if (isMoving) return;
        if (Shown == false) return;
        Shown = false;
        
        //m_FoldingPanel.gameObject.SetActive(false);
        StartCoroutine(PanelMoving(-1));
        m_ImageGroup.SetActive(false);
    }
    private Vector2 m_PanelOpenPos;
    //private Vector2 m_Panel
    private bool isMoving = false;
    private bool Shown = false;
    private IEnumerator PanelMoving(int dir)
    {
        isMoving = true;
        Vector2 pos = m_FoldingPanelRect.anchoredPosition;
        for(int i = 0; i < 100; i++)
        {
            pos.y += dir * 7;
            m_FoldingPanelRect.anchoredPosition = pos;
            yield return null;
        }
        if(dir == -1)
        {
            m_FoldingPanelRect.anchoredPosition = m_PanelOpenPos;
        }
        isMoving = false;
        
    }

    public void Show()
    {
        if (isMoving) return;
        if (Shown == true) return;
        Shown = true;
        
        //m_FoldingPanel.gameObject.SetActive(true);
        StartCoroutine(PanelMoving(1));
        m_ImageGroup.SetActive(true);

        
    }

    public void MoveArrow(Vector3 pos)
    {
        pos.y += 10;
        m_Arrow.transform.position = pos;
        //arrow.transform.rotation = Quaternion.Euler(0,0,0);
    }
    
    public void SelectLevel(LevelPivot pivot)
    {
        m_CurSelectedPivot = pivot;
        PopulateText(pivot.g_Text, pivot.g_Mode, pivot.g_Req);
        m_LevelName.text = m_CurSelectedPivot.g_LevelName ?? "NO SUCH LEVEL";
        m_ImageGroupContent.sprite = m_CurSelectedPivot.preview[0];
        m_CurSelected = pivot.g_ID;
        if(m_CurSelected > 1)
        {
            m_GoLevelButton.interactable = false;

        }
        else
        {
            m_GoLevelButton.interactable = true;
        }
        Show();
        m_SceneCamera.Switch(pivot.gameObject.transform.position);
        MoveArrow(pivot.gameObject.transform.position);
    }
    RectTransform m_IntroTextRect;
    RectTransform m_FoldingPanelRect;
    public void PopulateText(string intro, LevelPivot.Mode mode, int goal)
    {
        m_IntroText.text = intro;
        
        switch (mode)
        {
            case LevelPivot.Mode.DEATH_MATCH:
                m_ModeText.text = "Death Match";
                m_GoalText.text = "Kill enemies.";       
                break;
            case LevelPivot.Mode.SURVIVAL:
                m_ModeText.text = "Survival";
                m_GoalText.text = "Survive " + (int)(goal * m_Difficulty) + " secs.";
                break;
            case LevelPivot.Mode.INF:
                m_ModeText.text = "Infinity";
                m_GoalText.text = "Survive as long as you can.";
                break;
            case LevelPivot.Mode.BOSS:
                m_ModeText.text = "Boss Fight";
                m_GoalText.text = "Kill one boss.";
                break;
        }
        //m_IntroTextRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_IntroText.preferredHeight);
        //float desiredHeight = m_IntroText.preferredHeight + 200;
        //m_FoldingPanelRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, desiredHeight);
        //float desiredXPos = Random.Range(40, 100);
        //m_FoldingPanelRect.anchoredPosition = new Vector2(desiredXPos, -300f + desiredHeight * 0.4f);

    }
    public void ChangeDiffculty(float diff)
    {
        m_AudioSource.clip = m_Click;
        m_AudioSource.Play();
        m_Difficulty = diff;
        switch (m_CurSelectedPivot.g_Mode)
        {
            case LevelPivot.Mode.DEATH_MATCH:
                m_ModeText.text = "Death Match";
                m_GoalText.text = "Kill enemies.";
                break;
            case LevelPivot.Mode.SURVIVAL:
                m_ModeText.text = "Survival";
                m_GoalText.text = "Survive " + (int)(m_CurSelectedPivot.g_Req * m_Difficulty) + " secs.";
                break;
            case LevelPivot.Mode.INF:
                m_ModeText.text = "Infinity";
                m_GoalText.text = "Survive as long as you can.";
                break;
            case LevelPivot.Mode.BOSS:
                m_ModeText.text = "Boss Fight";
                m_GoalText.text = "Kill one Boss.";
                break;
        }
    }

    public void SelectDiffculty(int diff)
    {
        switch (diff)
        {
            case 0:
                ChangeDiffculty(1);
                m_Easy.interactable = false;
                m_Medium.interactable = true;
                m_Hard.interactable = true;
                m_LevelDiffculty.text = "[EASY]";
                m_LevelDiffculty.color = m_Easy.colors.disabledColor;
                m_ImageGroupContent.sprite = m_CurSelectedPivot.preview[0];
                break;
            case 1:
                ChangeDiffculty(1.4f);
                m_Easy.interactable = true;
                m_Medium.interactable = false;
                m_Hard.interactable = true;
                m_LevelDiffculty.text = "[NORMAL]";
                m_LevelDiffculty.color = m_Medium.colors.disabledColor;
                m_ImageGroupContent.sprite = m_CurSelectedPivot.preview[1];
                break;
            case 2:
                ChangeDiffculty(1.8f);
                m_Easy.interactable = true;
                m_Medium.interactable = true;
                m_Hard.interactable = false;
                m_LevelDiffculty.text = "[HARD]";
                m_LevelDiffculty.color = m_Hard.colors.disabledColor;
                m_ImageGroupContent.sprite = m_CurSelectedPivot.preview[2];
                break;
        }
    }
    public void GoNextLevel()
    {
        if (m_CurSelectedPivot.g_Next != null)
        {
            SelectLevel(m_CurSelectedPivot.g_Next);
        }
    }

    public void GoPrevious()
    {
        if (m_CurSelectedPivot.g_Previous != null)
        {
            SelectLevel(m_CurSelectedPivot.g_Previous);
        }
    }
    private void OnDestroy()
    {
        GameAssetsManager.instance.UpdateSave();
        GameUIManager.instance.RemoveUI();
    }

    public override void SendCommand(UICommand command)
    {
        throw new System.NotImplementedException();
    }
}
