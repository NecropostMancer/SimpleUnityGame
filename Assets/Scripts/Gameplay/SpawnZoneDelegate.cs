using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 当头文件使用。
 
 */
public static class SpawnZoneDelegate
{
    public delegate Vector3 getRandomPointGenerator( bool onSurface);
    public delegate void drawGizmo();
    public enum Type { SPHERE, CIRCLE, CUBE, RECT };
}
