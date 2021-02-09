using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : MonoBehaviour
{


    public Weapon m_currentWeapon;
    private int m_Index = -1;

    private Weapon[] m_Weapons;
    
    private GameObject[] m_WeaponPool;

    

    // Start is called before the first frame update
    void Start()
    {
        m_Weapons = BattleManager.instance.GetCurrentWeapon().ToArray();
        m_WeaponPool = new GameObject[m_Weapons.Length];
        if(m_Weapons.Length != 0)
        Switch(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Switch(int index)
    {
        if (index >= m_Weapons.Length) return;
        
        if (index == m_Index) return;
        m_Index = index;
        if(m_WeaponPool[index] == null)
        {
            m_WeaponPool[index] = Instantiate(m_Weapons[index].gameObject, transform);
            Utils.SetLayerRecursively(m_WeaponPool[index].gameObject, 8);//第一人称摄像机用，专拍武器，防止穿模
            m_WeaponPool[index].gameObject.SetActive(false);
        }
        if (m_currentWeapon != null)
        {
            m_currentWeapon.ClearAnimator();
            m_currentWeapon.gameObject.SetActive(false);
            
        }
        m_currentWeapon = m_WeaponPool[index].GetComponent<Weapon>();
        m_currentWeapon.gameObject.SetActive(true);
    }

    public void Aim(bool a)
    {
        m_currentWeapon.Aim(a);
        
    }

}
