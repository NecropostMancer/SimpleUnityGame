using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LoadingControl : MonoBehaviour
{

    public static LoadingControl sm_currentInstance;

    [SerializeField]
    private Image m_Fill;
    [SerializeField]
    private GameObject canvas;
    [SerializeField]
    private AudioListener audioListener;
    [SerializeField]
    private GameObject Fade;
    [SerializeField]
    private AnimationCurve fadeCurve;
    [SerializeField]
    private Camera sceneCamera;

    private bool m_EnableFadeIn;
    private bool m_EnableFadeOut;
    public void SetProgress(float progress)
    {
        m_Fill.fillAmount = progress;
        if(progress > 1f)
        {
            
                StartFadeOut();
            
        }
    }

    public void StartFadeOut()
    {
        if (!Fade.activeSelf)
        {
            Fade.SetActive(true);
        }
        m_Fill.gameObject.SetActive(false);
        canvas.SetActive(false);
        sceneCamera.enabled = false;
        StartCoroutine(FadeOutE());
        

    }

    public void StartFadeIn()
    {
        if (m_EnableFadeIn)
        {
            Fade.SetActive(true);
            StartCoroutine(FadeInE());
        }
        
    }
    private bool m_isFading;
    IEnumerator FadeOutE()
    {
        //GameObject go = canvas.transform.GetChild(0).gameObject;
        Image image = Fade.transform.GetChild(0).GetComponent<Image>();
        float time = 0;
        while (m_isFading)
        {
            yield return null;
        }
        if (m_EnableFadeOut)
        {
            while (time < 1f)
            {
                time += Time.deltaTime;
                image.color = new Color(image.color.r, image.color.r, image.color.r, 1f - fadeCurve.Evaluate(time*2));
                //Debug.Log(time);
                yield return null;
            }
        }
        Fade.SetActive(false);
        GameAssetsManager.instance.RequestLoadingEnd();
    }
    IEnumerator FadeInE()
    {
        //GameObject go = canvas.transform.GetChild(0).gameObject;
        Image image = Fade.transform.GetChild(0).GetComponent<Image>();
        float time = 0;
        m_isFading = true;
        while (time < 1f)
        {
            time += Time.deltaTime;
            image.color = new Color(image.color.r, image.color.r, image.color.r, fadeCurve.Evaluate(time*2));
            //Debug.Log(time);
            yield return null;
        }
        
        canvas.SetActive(true);
        sceneCamera.enabled = true;
        m_isFading = false;
        GameAssetsManager.instance.RequesetLoadingStart();
    }

    private void OnEnable()
    {
        //向scenemanager传达已准备好
        //GameObject.FindGameObjectWithTag("Manager").GetComponent<GameSceneManager>().;
    }
    
    public void SetSetting(bool enableFadeIn,bool enableFadeOut)
    {
        m_EnableFadeIn = enableFadeIn;
        m_EnableFadeOut = enableFadeOut;
    }
    private void Awake()
    {
        sm_currentInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Fade.SetActive(false);
        
    }

    private void OnDestroy()
    {
        sm_currentInstance = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAudioListenerActive(bool a)
    {
        audioListener.enabled = a;
    }
}
