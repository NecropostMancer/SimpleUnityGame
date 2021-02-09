using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MainMenuButton : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    [SerializeField]
    private Image m_shadow;
    [SerializeField]
    private Vector2 m_shadowOffset = new Vector2(1,-1f);
    
    private Color m_reserve;

    private TextMeshProUGUI m_text;
    [SerializeField]
    private RectTransform m_HintRoot;
    private RectTransform m_This;
    [SerializeField]
    private TextMeshProUGUI m_HintTextArea;
    public string m_MyHint;

    private AudioSource m_AudioSource;
    [SerializeField]
    private AudioClip m_Select;
    [SerializeField]
    private AudioClip m_Hover;
    //private Text m_text;
    public void OnPointerEnter(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        m_shadow.rectTransform.anchoredPosition = GetComponent<Image>().rectTransform.anchoredPosition + m_shadowOffset;
        m_shadow.enabled = true;
        m_text.color = Color.black;
        if (m_HintRoot)
        {
            Vector2 tmp = m_HintRoot.anchoredPosition;
            tmp.y = m_This.anchoredPosition.y;
            m_HintRoot.anchoredPosition = tmp;
            m_HintTextArea.text = m_MyHint;
            m_HintRoot.gameObject.SetActive(true);
        }
        m_AudioSource.clip = m_Hover;
        m_AudioSource.Play();
        //m_text.material.
        //m_text.color = Color.black;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_shadow.enabled = false;
        m_text.color = m_reserve;
        if (m_HintRoot)
        {
            
            m_HintRoot.gameObject.SetActive(false);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        m_AudioSource.clip = m_Select;
        m_AudioSource.Play();
    }
    // Start is called before the first frame update
    void Start()
    {
        
        m_text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        m_reserve = m_text.color;
        m_This = GetComponent<RectTransform>();
        m_AudioSource = GetComponent<AudioSource>();
        m_AudioSource.loop = false;
        AudioManager.instance.AudioRegister(m_AudioSource, AudioManager.AudioType.UISFX);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
