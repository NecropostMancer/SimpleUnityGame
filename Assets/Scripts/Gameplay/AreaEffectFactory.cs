using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 这个类原本用来辅助AreaEffect和Projectile的桥接，设想是
在projectile中直接用枚举类型拉一个列表，再选择对应的效果，
运行时将其实例化，目前没接着写。
 
 */
public static class AreaEffectFactory
{
    public enum EffectType
    {
        A,B,C
    }

    public static AreaEffect GetEffectInstance(EffectType ef)
    {
        return new AreaEffect();
    }
}
