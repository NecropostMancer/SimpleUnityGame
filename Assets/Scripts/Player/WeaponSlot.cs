using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : MonoBehaviour
{


    public Weapon m_currentWeapon;
    [SerializeField]
    private Weapon[] m_Weapons;
    
    private GameObject[] m_WeaponPool;

    

    // Start is called before the first frame update
    void Start()
    {
        
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
        index %= m_Weapons.Length;
        //Destroy(currentWeapon.gameObject);
        //currentWeapon = Weapons[index];
        
        if(transform.childCount != 0)
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
        
        m_currentWeapon = Instantiate(m_Weapons[index].gameObject, transform).GetComponent<Weapon>();
        Utils.SetLayerRecursively(m_currentWeapon.gameObject, 8);//第一人称摄像机用，专拍武器，防止穿模
    }

    private GameObject LoadWeapon(int index)
    {
        if(m_WeaponPool[index] == null)
        {
            m_WeaponPool[index] = m_Weapons[index].gameObject;
        }
        return m_WeaponPool[index];
    }

    public void Aim(bool a)
    {
        m_currentWeapon.isAiming = a;
    }

}
