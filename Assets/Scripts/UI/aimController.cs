using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AimController : MonoBehaviour
{
    // 依赖于prefab的层级结构。
    private RectTransform[] aim = new RectTransform[4];

    private bool _hit = false;
    private GameObject hitEffect;

    private int sinceLastHit = 0;
    private bool hit
    {
        set
        {
            if (value)
            {
                hitEffect.SetActive(true);
                sinceLastHit = 0;
            }
            else
            {
                hitEffect.SetActive(false);
            }
            _hit = value;
        }
        get
        {
            return _hit;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        hitEffect = transform.GetChild(4).gameObject;
        _hit = false;
        hitEffect.SetActive(false);
        for(int i = 0; i < 4; i++)
        {
            aim[i] = transform.GetChild(i).GetComponent<RectTransform>();
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

    public void Expand(float str)
    {
        str *= 30;
        aim[0].anchoredPosition = new Vector3(0, str, 0);
        aim[1].anchoredPosition = new Vector3(str, 0, 0);
        aim[2].anchoredPosition = new Vector3(0, -str, 0);
        aim[3].anchoredPosition = new Vector3(-str, 0, 0);
    }

    public void Hit()
    {
        hit = true;
    }

    
}
