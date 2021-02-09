using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ProfileData
{
    [SerializeField]
    private float _healthBonus;
    public float healthBonus
    {
        set
        {
            if (value < 1)
            {
                value = 1.0f;
            }
            _healthBonus = value;
        }
        get
        {
            if(_healthBonus < 1)
            {
                _healthBonus = 1;
            }
            return _healthBonus;
        }
    }
    [SerializeField]
    private float _attackBonus;
    public float attackBonus
    {
        set
        {
            if (value < 1)
            {
                value = 1.0f;
            }
            _attackBonus = value;
        }
        get
        {
            if (_attackBonus < 1)
            {
                _attackBonus = 1;
            }
            return _attackBonus;
        }
    }
    [SerializeField]
    private float _reloadingBonus;
    public float reloadingBonus
    {
        set
        {
            if (value < 1)
            {
                value = 1.0f;
            }
            _reloadingBonus = value;
        }
        get
        {
            if (_reloadingBonus < 1)
            {
                _reloadingBonus = 1;
            }
            return _reloadingBonus;
        }
    }
    [SerializeField]
    private int _money;
    public int money
    {
        set
        {
            if (value < 0)
            {
                value = 0;
            }
            _money = value;
        }
        get
        {
            if (_money < 0)
            {
                _money = 0;
            }
            return _money;
        }
    }
    public int level;
    public bool[] unlockedWeapon = new bool[8];


    public static void ResetProfile(ProfileData data)
    {
        data.reloadingBonus = data.attackBonus = data.healthBonus = 1;
        data.money = 99999;
        data.level = 1;
        data.unlockedWeapon = new bool[8];
        data.unlockedWeapon[0] = true;
    }
}
