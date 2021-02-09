using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Slider HP;
    private Color HPFullColor;
    [SerializeField]
    private Color HPDangerColor;
    [SerializeField]
    private Animator Blink;
    private Image HPFill;
    private float hp;
    private float maxHp;
    public Slider Armor;
    private float armor;
    private float maxArmor;
    // Start is called before the first frame update
    void Start()
    {
        HPFill = HP.fillRect.GetComponent<Image>();
        HPFullColor = HPFill.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private float m_Ratio;
    public void ModHP(float v)
    {
        hp += v;
        if (hp > maxHp) { hp = maxHp; }
        m_Ratio = hp / maxHp;
        HPFill.color = m_Ratio * HPFullColor + (1 - m_Ratio) * HPDangerColor;
        Blink.SetTrigger("change");
        HP.value = m_Ratio;
    } 
    

    public void SetHP(float v)
    {
        hp = v;
        if (hp > maxHp) { hp = maxHp; }
        m_Ratio = hp / maxHp;
        HPFill.color = m_Ratio * HPFullColor + (1 - m_Ratio) * HPDangerColor;
        Blink.SetTrigger("change");
        HP.value = m_Ratio;
    }

    public void ModArmor(float v)
    {
        armor += v;
        if (armor > maxArmor) { armor = maxArmor; }
        Armor.value = armor / maxArmor;
    }

    public void ResetArmor(float v)
    {
        armor = v;
        if (armor > maxArmor) { armor = maxArmor; }
        Armor.value = armor / maxArmor;
    }

    public void SetMax(float _maxHP,float _maxArmor)
    {
        maxHp = _maxHP;
        maxArmor = _maxArmor;
    }
}
