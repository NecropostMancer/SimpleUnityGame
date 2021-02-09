using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPivot : MonoBehaviour
{
    public int g_ID;
    [TextArea]
    public string g_Text;
    public LevelPivot g_Next;
    public LevelPivot g_Previous;
    public enum Mode{DEATH_MATCH, SURVIVAL, INF, BOSS }
    public Mode g_Mode;
    public int g_Req;
    public string g_LevelName;

    public Sprite[] preview = new Sprite[3]; 

    public AudioClip[] g_Track;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
