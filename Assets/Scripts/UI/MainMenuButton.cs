using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MainMenuButton : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField]
    private Image m_shadow;
    [SerializeField]
    private Vector2 m_shadowOffset = new Vector2(1,-1f);
    
    private Color m_reserve;
    

    private TextMeshProUGUI m_text;
    //private Text m_text;
    public void OnPointerEnter(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        m_shadow.rectTransform.anchoredPosition = GetComponent<Image>().rectTransform.anchoredPosition + m_shadowOffset;
        m_shadow.enabled = true;
        m_text.color = Color.black;
        //m_text.material.
        //m_text.color = Color.black;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_shadow.enabled = false;
        m_text.color = m_reserve;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        m_text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        m_reserve = m_text.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
