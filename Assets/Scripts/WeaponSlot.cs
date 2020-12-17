using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : MonoBehaviour
{


    private Weapon currentWeapon;


    //一个问题：加入到这里的东西开始运行后已经实例化了吗？
    [SerializeField]
    private Weapon[] Weapons;

    
    private GameObject[] WeaponPool;
    // Start is called before the first frame update
    void Start()
    {
        WeaponPool = new GameObject[Weapons.Length];
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Switch(int index)
    {
        index %= Weapons.Length;
        //Destroy(currentWeapon.gameObject);
        currentWeapon = Weapons[index];
        
        if(transform.childCount != 0)
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
        
        Instantiate(Weapons[index].gameObject, transform);
    }

    private void LoadWeapon(int index)
    {
        if(WeaponPool[index] == null)
        {
            WeaponPool[index] = Weapons[index].gameObject;
        }
    }

}
