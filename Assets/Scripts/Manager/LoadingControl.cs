using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LoadingControl : MonoBehaviour
{



    Material material;
    [SerializeField]
    private GameObject gunmodel;
    [SerializeField]
    private GameObject canvas;
    [SerializeField]
    private GameObject Fade;
    [SerializeField]
    private AnimationCurve fadeCurve;
    [SerializeField]
    private Camera sceneCamera;
    public void SetProgress(float progress)
    {
        if (material)
        {
            material.SetFloat(Shader.PropertyToID("Ratio"), progress);
        }
    }

    public void StartFadeOut(GameAssetsManager refer)
    {
        Fade.SetActive(true);
        sceneCamera.depth = -100;
        StartCoroutine(FadeOutE(refer));
        gunmodel.SetActive(false);
        canvas.SetActive(false);

    }

    IEnumerator FadeOutE(GameAssetsManager refer)
    {
        //GameObject go = canvas.transform.GetChild(0).gameObject;
        Image image = Fade.transform.GetChild(0).GetComponent<Image>();
        float time = 0;
        
        while (time < 2f)
        {
            time += Time.deltaTime;
            image.color = new Color(image.color.r, image.color.r, image.color.r, 1f - fadeCurve.Evaluate(time));
            //Debug.Log(time);
            yield return null;
        }
        refer.RequestLoadingEnd();
    }

    private void OnEnable()
    {
        //向scenemanager传达已准备好
        //GameObject.FindGameObjectWithTag("Manager").GetComponent<GameSceneManager>().;
    }

    // Start is called before the first frame update
    void Start()
    {
        material = gunmodel.GetComponent<MeshRenderer>().material;
        Fade.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
