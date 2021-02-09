using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//on camera
public class LevelSelectionCamera : MonoBehaviour
{

    private Vector3 lookat;

    private Camera thisCamera;
    [SerializeField]
    private GameObject m_InitWith;
    private Vector3 pivot;

    private Vector3 distence;

    private GraphicRaycaster graphicRaycaster;
    // Start is called before the first frame update
    void Start()
    {
        thisCamera = GetComponent<Camera>();
        pivot = m_InitWith.transform.position;
        transform.LookAt(pivot);
        distence = transform.position - pivot;
        this.transform.parent = null;
    }
    float m_PauseTime = 0;
    Vector3 m_Temp;
    // Update is called once per frame
    void Update()
    {
        if (m_PauseTime < 0)
        {
            transform.RotateAround(pivot, Vector3.up, Time.deltaTime);
        }
        else
        {
            m_PauseTime -= Time.deltaTime;
        }
        if (Input.GetMouseButton(1))
        {
            m_PauseTime = 3f;
            transform.RotateAround(pivot, Vector3.up, Input.GetAxis("Mouse X") * 1.5f);
            
        }
        
        m_Temp = transform.position + transform.forward * Input.mouseScrollDelta.y * 3f;
        if ((m_Temp - pivot).y > 3 && (m_Temp - pivot).y < 100)
        {
            transform.position = m_Temp;
        }
    }

    private bool m_isSwitching = false;
    IEnumerator GoNewPivot(Vector3 camPos, Vector3 newPivot, Vector3 oldPivot)
    {
        Vector3 delta = newPivot - camPos + distence;
        
        
        float timeDelta = 0;
        while(timeDelta < 2)
        {
            timeDelta += Time.deltaTime;
            float step = Mathf.SmoothStep(0, 2, timeDelta);
            transform.position = camPos + step * 0.5f * delta;
            transform.LookAt((newPivot - oldPivot) * 0.5f * step + oldPivot);
            //transform.rotation.SetLookRotation( - transform.position);
            yield return null;
        }
        m_isSwitching = false;
        
    }

    public void Switch(Vector3 pos)
    {
        m_PauseTime = -1;
        if (m_isSwitching) {
            StopAllCoroutines();
        }
        
        m_isSwitching = true;
        StartCoroutine(GoNewPivot(transform.position, pos, pivot));
        pivot = pos;
        
    }
}
