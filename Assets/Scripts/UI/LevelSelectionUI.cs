using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSelectionUI : MonoBehaviour
{
    public LevelSelectionCamera sceneCamera;
    public GameObject m_foldingPanel;
    public TextMeshProUGUI m_introText;
    public Button m_goStoreButton;
    public Button m_goLevelButton;
    public GameObject m_arrow;

    private int m_curSelected;
    void Start()
    {
        m_goStoreButton.onClick.AddListener(GoStore);
        m_goLevelButton.onClick.AddListener(GoLevel);
    }
    private void GoStore()
    {
        GameAssetsManager.instance.LoadSceneByName("Store");
    }
    private void GoLevel()
    {
        GameAssetsManager.instance.LoadSceneByName("BattleScene"+m_curSelected.ToString());
    }
    // Update is called once per frame
    void Update()
    {

        //Quaternion q = Quaternion.LookRotation();
        //q = Quaternion.Euler(0, 0, -90) * q;
        //arrow.transform.LookAt(arrowDir);
        //arrow.transform.Rotate(arrowDir - arrow.transform.position, 90);
        Vector3 arrowDir = new Vector3(sceneCamera.transform.position.x, m_arrow.transform.position.y, sceneCamera.transform.position.z);
        Debug.DrawLine(arrowDir, m_arrow.transform.position);
        m_arrow.transform.LookAt(arrowDir);
        //arrow.transform.Rotate(arrowDir - arrow.transform.position, 90);
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            if (results.Count != 0)
            {
                bool isUI = false;
                foreach (RaycastResult hit in results)
                {
                    if (hit.gameObject.GetComponent<Image>() != null)
                    {
                        isUI = true;
                        break;
                    }
                }
                if (!isUI)
                {
                    foreach (RaycastResult hit in results)
                    {
                        LevelPivot levelPivot = hit.gameObject.GetComponent<LevelPivot>();
                        if (levelPivot)
                        {
                            PopulateText(levelPivot.text);
                            m_curSelected = levelPivot.id;
                            Show();
                            sceneCamera.Switch(hit.gameObject.transform.position);
                            MoveArrow(hit.gameObject.transform.position);
                            return;
                        }

                    }
                    Hide();
                }


            }
        }
    }

    public void Hide()
    {
        m_foldingPanel.gameObject.SetActive(false);
    }

    public void Show()
    {
        m_foldingPanel.gameObject.SetActive(true);
    }

    public void MoveArrow(Vector3 pos)
    {
        pos.y += 10;
        m_arrow.transform.position = pos;
        //arrow.transform.rotation = Quaternion.Euler(0,0,0);
    }

    public void PopulateText(string s)
    {
        m_introText.text = s;
    }
}
