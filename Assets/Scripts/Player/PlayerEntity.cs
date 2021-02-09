using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : MonoBehaviour
{
    public float m_hp;
    public float m_armor;

    public HPBar m_HPBar;
    // Start is called before the first frame update
    void Start()
    {
        CharacterManager.instance.AddUnitReference(this);
        m_hp = 100 * GameAssetsManager.instance.GetSave().healthBonus;
        m_armor = 0;
        m_HPBar.SetMax(m_hp, m_armor);
        
    }
    public bool Damaged(float v)
    {
        m_hp -= v;
        m_HPBar.ModHP(-v);
        if(m_hp < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public void ReFill()
    {
        m_HPBar.ModHP(999);
        m_HPBar.ModArmor(999);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
