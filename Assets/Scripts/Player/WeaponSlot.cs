using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : MonoBehaviour
{


    public Weapon currentWeapon;


    
    [SerializeField]
    private Weapon[] Weapons;

    
    private GameObject[] WeaponPool;

    // Start is called before the first frame update
    void Start()
    {
        WeaponPool = new GameObject[Weapons.Length];
        if(Weapons.Length != 0)
        Switch(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Switch(int index)
    {
        index %= Weapons.Length;
        //Destroy(currentWeapon.gameObject);
        //currentWeapon = Weapons[index];
        
        if(transform.childCount != 0)
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
        
        currentWeapon = Instantiate(Weapons[index].gameObject, transform).GetComponent<Weapon>();
        SetLayerRecursively(currentWeapon.gameObject, 8);//第一人称摄像机用，专拍武器，防止穿模
    }

    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (null == obj)
        {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    private GameObject LoadWeapon(int index)
    {
        if(WeaponPool[index] == null)
        {
            WeaponPool[index] = Weapons[index].gameObject;
        }
        return WeaponPool[index];
    }

}
