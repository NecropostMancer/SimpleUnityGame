using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelStart : MonoBehaviour
{
    private float m_Timer;
    private Animation m_Animation;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 4f);
        gameObject.GetComponent<TextMeshProUGUI>().enabled = false;
        m_Animation = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Timer += Time.deltaTime;
        if (m_Timer > 1)
        {
            gameObject.GetComponent<TextMeshProUGUI>().enabled = true;
            m_Animation.Play();
            this.enabled = false;
        }
    }
}
