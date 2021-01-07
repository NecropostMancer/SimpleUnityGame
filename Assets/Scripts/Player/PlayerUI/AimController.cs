using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AimController : MonoBehaviour
{
    // 依赖于prefab的层级结构。
    private RectTransform[] m_aim = new RectTransform[4];

    private bool m_hit = false;
    private GameObject m_hitEffect;
    private float m_curAcc;

    private int sinceLastHit = 0;
    private bool hit
    {
        set
        {
            if (value)
            {
                m_hitEffect.SetActive(true);
                sinceLastHit = 0;
            }
            else
            {
                m_hitEffect.SetActive(false);
            }
            m_hit = value;
        }
        get
        {
            return m_hit;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        m_hitEffect = transform.GetChild(4).gameObject;
        m_hit = false;
        m_hitEffect.SetActive(false);
        for(int i = 0; i < 4; i++)
        {
            m_aim[i] = transform.GetChild(i).GetComponent<RectTransform>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        sinceLastHit++;
        if(sinceLastHit > 75)
        {
            hit = false;
        }
    }
    //aimCommand.str = Mathf.Clamp((1f - accuracy) * 10 + curRecoil / (curRecoil + 20) * 10,1,10);
    //aimCommand.str = Mathf.Clamp((1f - accuracy) * 10, 1, 10);
    public void Expand(float str)
    {
        str = Mathf.Clamp((1f - m_curAcc) * 10 + str / (str + 20) * 10, 1, 10);
        str *= 30;
        m_aim[0].anchoredPosition = new Vector3(0, str, 0);
        m_aim[1].anchoredPosition = new Vector3(str, 0, 0);
        m_aim[2].anchoredPosition = new Vector3(0, -str, 0);
        m_aim[3].anchoredPosition = new Vector3(-str, 0, 0);
    }

    public void ResetAim(float defaultAcc)
    {
        m_curAcc = defaultAcc;
    }

    public void Hit()
    {
        hit = true;
    }

    
}
