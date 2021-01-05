using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//attach to camera.
public class MainMenuVFX : MonoBehaviour
{
    private Vector2 camPos;
    private void Start()
    {
        camPos.x = this.transform.position.x;
        camPos.y = this.transform.position.y;
    }

    private void Update()
    {
        Vector2 pos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
        pos.x = Mathf.Clamp((pos.x / Screen.width) - 0.5f,-0.5f,0.5f) * 0.05f;
        pos.y = Mathf.Clamp((pos.y / Screen.height) - 0.5f,-0.5f,0.5f) *0.05f;
        
        gameObject.transform.position = new Vector3(camPos.x + pos.x, camPos.y + pos.y, -10);
    }
}
