using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    
    private List<Weapon> m_AvaliableWeapons;
    [SerializeField]
    private List<int> m_WeaponPrice;

    private ProfileData m_ProfileData;

    private readonly Dictionary<WeaponDescriptor, int> m_Table = new Dictionary<WeaponDescriptor, int>();
    private readonly Dictionary<WeaponDescriptor, int> m_DesToIndex = new Dictionary<WeaponDescriptor, int>();

    private List<int> m_PlayerStatsLevel = new List<int>();

    private bool m_ReadyToRead = false;
    // Start is called before the first frame update
    void Start()
    {
        m_ProfileData = GameAssetsManager.instance.GetSave();
        m_AvaliableWeapons = BattleManager.instance.GetAllWeapon();
        TranslateStatsToLevels();
        if (m_AvaliableWeapons.Count == m_WeaponPrice.Count)
        {
            for(int i = 0; i < m_WeaponPrice.Count; i++)
            {
                if(!m_ProfileData.unlockedWeapon[i])
                m_Table.Add(m_AvaliableWeapons[i].getDescriptor(), m_WeaponPrice[i]);
                m_DesToIndex.Add(m_AvaliableWeapons[i].getDescriptor(), i);
            }
        }
#if UNITY_EDITOR
        else
        {
            Debug.Log("Store: Not enough prices defined.");
        }
#endif
        m_ReadyToRead = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Dictionary<WeaponDescriptor,int> GetOnSaleWeapon()
    {
        return m_Table;
    }
    
    public List<int> GetPlayerStats()
    {
        
        return m_PlayerStatsLevel;
    }

    public bool BuyWeapon(WeaponDescriptor weapon)
    {
        if (m_ProfileData.money - m_Table[weapon] >= 0)
        {
            m_ProfileData.money -= m_Table[weapon];
            m_Table.Remove(weapon);
            //BattleManager.instance.EnableWeaponForPlayer(m_DesToIndex[weapon]);
            //int a = m_avaliableWeapons.FindIndex(a => a == weapon);
            m_ProfileData.unlockedWeapon[m_DesToIndex[weapon]] = true;
            
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool BuyLevel(int index)
    {
        if(m_ProfileData.money < 250)
        {
            
            return false;
            
        }
        m_PlayerStatsLevel[index]++;
        m_ProfileData.money -= 250;
        switch (index)
        {
            case 0:
                m_ProfileData.healthBonus += 0.2f;
                break;
            case 1:
                m_ProfileData.reloadingBonus += 0.1f;
                break;
            case 2:
                m_ProfileData.attackBonus += 0.2f;
                break;
            default:
                break;

        }
        return true;
    }

    private void TranslateStatsToLevels()
    {
        float hpb = m_ProfileData.healthBonus;
        float rlb = m_ProfileData.reloadingBonus;
        float attb = m_ProfileData.attackBonus;
        m_PlayerStatsLevel.Add((int)((hpb - 1.0) / 0.2 + 0.5));
        m_PlayerStatsLevel.Add((int)((rlb - 1.0) / 0.1 + 0.5));
        m_PlayerStatsLevel.Add((int)((attb - 1.0) / 0.2 + 0.5));
    }

    public bool isReady()
    {
        return m_ReadyToRead;
    }

    public int GetMoney()
    {
        return m_ProfileData.money;
    }
}
