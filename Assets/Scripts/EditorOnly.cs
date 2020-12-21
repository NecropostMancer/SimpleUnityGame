using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorOnly : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Camera cam = (Camera)GameObject.Find("Main Camera").GetComponent<Camera>();
        if(cam.depth > 0)
        {
            cam.depth = -9;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
