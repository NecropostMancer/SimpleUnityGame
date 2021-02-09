using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class GameSetting
{
    public float[] volume = new float[4];
    public float sen = 1.0f;

    public static void ResetAll(GameSetting gameSetting)
    {
        gameSetting.volume[0] = 0;
        gameSetting.volume[1] = 0;
        gameSetting.volume[2] = 0;
        gameSetting.volume[3] = 0;
        gameSetting.sen = 1;
    }
}
