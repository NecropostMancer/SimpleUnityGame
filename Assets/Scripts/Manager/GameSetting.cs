using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSetting
{
    public static Resolution currentResolution = Screen.currentResolution;
    public static Vector2 centerPoint = new Vector2(currentResolution.width/2,currentResolution.height/2);


}
