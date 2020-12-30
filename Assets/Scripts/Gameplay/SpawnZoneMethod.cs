using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpawnZoneMethod
{
    public static SpawnZoneDelegate.getRandomPointGenerator NewGenerator(SpawnZoneDelegate.Type type)
    {
        switch (type)
        {
            case SpawnZoneDelegate.Type.SPHERE:
                return Type1;
                break;
            case SpawnZoneDelegate.Type.CIRCLE:
                return Type2;
                break;
            case SpawnZoneDelegate.Type.CUBE:
                return Type3;
                break;
            case SpawnZoneDelegate.Type.RECT:
                return Type4;
                break;
            default:
                return Type1;
                break;
        }
    }

    //SPHERE
    public static Vector3 Type1(bool onSurface)
    {
        if (onSurface)
        {
            return Random.onUnitSphere;
        }
        else
        {
            return Random.insideUnitSphere;
        }
    }

    //CIRCLE
    public static Vector3 Type2(bool onSurface)
    {
        Vector2 rand;
        if (onSurface)
        {
            rand = Random.insideUnitCircle;
            rand.Normalize();
            return new Vector3(rand.x, 0, rand.y);
        }
        else
        {
            rand = Random.insideUnitCircle;
            return new Vector3(rand.x, 0, rand.y);
        }
    }

    //CUBE
    public static Vector3 Type3(bool onSurface)
    {
        Vector3 p;
        p.x = Random.Range(-.5f, .5f);
        p.y = Random.Range(-.5f, .5f);
        p.z = Random.Range(-.5f, .5f);
        if (onSurface)
        {
            p.Normalize();
        }
        return p;
    }

    //RECT
    public static Vector3 Type4(bool onSurface)
    {
        Vector3 p;
        p.x = Random.Range(-.5f, .5f);
        p.y = 0f;
        p.z = Random.Range(-.5f, .5f);
        if (onSurface)
        {
            p.Normalize();
        }
        return p;
    }

}
