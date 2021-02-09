using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreUIBundle : UIBundle
{
    public GameObject m_scrollViewRoot;
    public GameObject m_fixedWeaponDataPanelRoot; //读入固定的宽，好安排可变部分的长度

    public TextMeshProUGUI m_money;

    public Button goWeaponGroup;
    public GameObject weaponGroup;
    
    public Transform contentRoot;
    public Button itemTemplate;

    public Transform weaponDetailRoot;
    public Camera weaponCamera;

    public Button quit;

    public Button goStatsGroup;
    public GameObject statsGroup;

    public Transform statsRoot;
    public TextMeshProUGUI intro;

    public Store dataSource;
    private Dictionary<WeaponDescriptor, int> data;

    public GameObject filledTemplate;
    public GameObject unfilledTemplate;
    public GameObject m_SoldOut;

    private RectTransform m_canvasTransform;

    private AudioSource m_AudioSource;
    public override void SendCommand(UICommand command)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_canvasTransform = GetComponent<RectTransform>();
        ReScale();
        m_AudioSource = GetComponent<AudioSource>();
        OpenWeaponGroup();

        goWeaponGroup.onClick.AddListener(OpenWeaponGroup);
        goStatsGroup.onClick.AddListener(OpenStatsGroup);
        quit.onClick.AddListener(Quit);

        
        AudioManager.instance.AudioRegister(m_AudioSource, AudioManager.AudioType.UISFX);


        itemTemplate.gameObject.SetActive(false);
        filledTemplate.SetActive(false);
        unfilledTemplate.SetActive(false);
        StartCoroutine(WaitForDataSourceReady());
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Screen.width);
        ReScale();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
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

    private void OpenWeaponGroup()
    {
        statsGroup.SetActive(false);
        weaponGroup.SetActive(true);
        goWeaponGroup.interactable = false;
        goStatsGroup.interactable = true;
        m_AudioSource.Play();
    }

    private void OpenStatsGroup()
    {
        weaponGroup.SetActive(false);
        statsGroup.SetActive(true);
        goStatsGroup.interactable = false;
        goWeaponGroup.interactable = true;
        m_AudioSource.Play();
    }

    private void Quit()
    {
        m_AudioSource.Play();
        GameAssetsManager.instance.LoadSceneByName("LevelSelection");
        
    }
    //brute.
    private void RefreshWeaponTab()
    {
        data = dataSource.GetOnSaleWeapon();
        float height = itemTemplate.GetComponent<RectTransform>().rect.height;
        float padding = 1;
        int index = 0;
        foreach(Transform child in contentRoot)
        {
            if (child.gameObject.activeInHierarchy)
            {
                Destroy(child.gameObject);
            }
        }
        if(data.Count == 0)
        {
            m_SoldOut.SetActive(true);
            return;
        }
        else
        {
            
        }
        foreach(var pair in data)
        {
            GameObject go = Instantiate(itemTemplate.gameObject,contentRoot);
            RectTransform trans = go.GetComponent<RectTransform>();
            Vector2 anchoredPos = trans.anchoredPosition;
            anchoredPos.y = -(height * index + (padding - 1) * index);
            trans.anchoredPosition = anchoredPos;
            DataHolder dataHolder = go.GetComponent<DataHolder>();
            dataHolder.AddField("Weapon", pair.Key);
            dataHolder.AddField("Price", pair.Value);
            ButtonEvents buttonEvents = go.GetComponent<ButtonEvents>();
            TextMeshProUGUI[] texts = go.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = pair.Key.name;
            texts[1].text = pair.Value.ToString();
            buttonEvents.onPointerEnter = delegate
            {
                ShowDetail(pair.Key);
            };
            buttonEvents.onPointerExit = delegate
            {
                ClearDetail();
            };
            buttonEvents.onPointerClick = delegate
            {
                BuyWeapon(pair.Key, pair.Value);
                m_AudioSource.Play();
                ClearDetail();
            };
            go.SetActive(true);
            index++;
        }

    }
    //private WeaponDescriptor m_currentSelected;
    private void onItem()
    {

    }
    private void ShowDetail(WeaponDescriptor m_currentSelected)
    {
        weaponDetailRoot.GetChild(0).GetComponent<TextMeshProUGUI>().text = m_currentSelected.name;
        weaponDetailRoot.GetChild(1).GetChild(4).GetComponent<TextMeshProUGUI>().text = m_currentSelected.damage.ToString();
        weaponDetailRoot.GetChild(1).GetChild(5).GetComponent<TextMeshProUGUI>().text = m_currentSelected.fireRate.ToString();
        weaponDetailRoot.GetChild(1).GetChild(6).GetComponent<TextMeshProUGUI>().text = m_currentSelected.ammo.ToString();
        weaponDetailRoot.GetChild(1).GetChild(7).GetComponent<TextMeshProUGUI>().text = m_currentSelected.acc.ToString();
    }
    private void ClearDetail()
    {
        weaponDetailRoot.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
        weaponDetailRoot.GetChild(1).GetChild(4).GetComponent<TextMeshProUGUI>().text = "";
        weaponDetailRoot.GetChild(1).GetChild(5).GetComponent<TextMeshProUGUI>().text = "";
        weaponDetailRoot.GetChild(1).GetChild(6).GetComponent<TextMeshProUGUI>().text = "";
        weaponDetailRoot.GetChild(1).GetChild(7).GetComponent<TextMeshProUGUI>().text = "";
    }

    private void BuyWeapon(WeaponDescriptor m_currentSelected,int price)
    {
        if (dataSource.BuyWeapon(m_currentSelected))
        {
            RefreshWeaponTab();
            m_money.text = "$:" + dataSource.GetMoney();
        }
        else
        {
            NotEnoughMoney();
        }
    }

    private void NotEnoughMoney()
    {
        StartCoroutine(NotEnoughMoneyAnimation());
    }
    private IEnumerator NotEnoughMoneyAnimation()
    {
        Color color = m_money.color;
        for(int i = 0; i < 3; i++)
        {
            m_money.color = Color.red;

            yield return new WaitForSeconds(0.2f);
            m_money.color = color;
        }
    }
    private static string[] des = { "+20% hp per level.","+10% reloading speed per level","+20% attack damage per level" };
    private void RefreshStatsTab()
    {
        Vector2 startAt = filledTemplate.GetComponent<RectTransform>().anchoredPosition;
        float vertGap = -90f;
        float horiGap = 38f;
        foreach (Transform child in statsRoot)
        {
            if (child.gameObject.activeInHierarchy)
            {
                Destroy(child.gameObject);
            }
        }
        for (int i = 0; i < 3; i++)
        {
            int j = 0;
            for(; j < dataSource.GetPlayerStats()[i]; j++)
            {
                GameObject go = Instantiate(filledTemplate, statsRoot);//too many images, should be one repeated image extending at width.
                go.GetComponent<RectTransform>().anchoredPosition = startAt + new Vector2(horiGap * j,vertGap * i);
                go.SetActive(true);
            }
            if (j > 4)
            {
                continue;
            }
            GameObject go2 = Instantiate(unfilledTemplate, statsRoot);
            go2.GetComponent<RectTransform>().anchoredPosition = startAt + new Vector2(horiGap * j, vertGap * i);
            go2.SetActive(true);
            ButtonEvents buttonEvents = go2.GetComponent<ButtonEvents>();
            int n = i;
            buttonEvents.onPointerClick = delegate
            {
                BuyStats(n);
                m_AudioSource.Play();
            };
            buttonEvents.onPointerEnter = delegate
            {
                intro.text = des[n];
            };
            buttonEvents.onPointerExit = delegate
            {
                intro.text = "Hover on available upgrades to preview stats (250$)";
            };
        }
    }

    private void BuyStats(int index)
    {
        if (dataSource.BuyLevel(index))
        {
            RefreshStatsTab();
            m_money.text = "$:" + dataSource.GetMoney();
        }
        else
        {
            NotEnoughMoney();
        }
    }

    IEnumerator WaitForDataSourceReady()
    {
        if(dataSource == null)
        {
            yield break;
        }
        while (!dataSource.isReady())
        {
            yield return null;
        }
        RefreshWeaponTab();
        RefreshStatsTab();
        m_money.text = "$:" + dataSource.GetMoney();
        yield break;
    }
}
