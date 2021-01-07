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
    private Vector3 pivot;

    private Vector3 distence;

    private GraphicRaycaster graphicRaycaster;
    // Start is called before the first frame update
    void Start()
    {
        thisCamera = GetComponent<Camera>();
        pivot = new Vector3(801f, 4.6f, 676.32f);
        transform.LookAt(pivot);
        distence = transform.position - pivot;
        this.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(pivot, Vector3.up, 0.2f);
        if ((pivot - transform.position).sqrMagnitude > 10000)
        {

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
        if (m_isSwitching) return;
        m_isSwitching = true;
        StartCoroutine(GoNewPivot(transform.position, pos, pivot));
        pivot = pos;
    }
}
