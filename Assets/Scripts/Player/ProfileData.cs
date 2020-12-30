using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public static class ProfileData 
{
    public static float healthBonus;
    public static float attackBonus;
    public static float reloadingBonus;
    public static int money;
    public static int level;
    public static bool[] unlockedWeapon = new bool[8];

    public static void Reset()
    {
        reloadingBonus = attackBonus = healthBonus = 1;
        money = 0;
        level = 1;
        unlockedWeapon = new bool[8];
        unlockedWeapon[0] = true;
    }
}
