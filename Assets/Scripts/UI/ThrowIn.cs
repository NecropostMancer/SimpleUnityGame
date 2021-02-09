using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ThrowIn : MonoBehaviour
{
    private RectTransform m_RectTransform;
    private Vector3 m_OriginScale;
    private Vector3 temp;

    public bool g_Start = false;
    // Start is called before the first frame update
    void Start()
    {
        m_RectTransform = GetComponent<RectTransform>();
        m_OriginScale = m_RectTransform.localScale;
        temp = new Vector3(1.4f * m_OriginScale.x, 1.4f * m_OriginScale.y, 1.0f);

        m_RectTransform.localScale = temp;

        temp = new Vector3(1.4f * m_OriginScale.x, 1.4f * m_OriginScale.y, 1.0f);
        m_RectTransform.localScale = temp;
        //this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!g_Start) return;
        if (temp.x >= 1f)
        {
            temp.x -= 0.01f;
            temp.y -= 0.01f;
            m_RectTransform.localScale = temp;
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnEnable()
    {
        
    }
}
