using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoldier_VR : EnemySoldier
{

    private void Start()
    {
        base.Start();
        m_AllowBodyClear = false;
        m_MaxPauseTime = Mathf.Max(m_DeadParticle.main.duration,2f);
    }
    [SerializeField]
    private ParticleSystem m_DeadParticle;
    [SerializeField]
    private AudioClip[] m_VRDissapearSounds;

    private float m_FloatingTime;
    private float m_MaxPauseTime;
    protected override void AfterDeath()
    {
        base.AfterDeath();
        for (int i = 0; i < bodies.Length; i++)
        {
            
            bodies[i].useGravity = false;
        }
        
        
    }

    private bool m_PlayStarted = false;
    
    private void Update()
    {
        base.Update();
        if (IsDead()) {
            m_FloatingTime += Time.deltaTime;
            if (!m_PlayStarted && m_FloatingTime > 3f)
            {
                m_PlayStarted = true;
                
                if (m_VRDissapearSounds != null && m_VRDissapearSounds.Length != 0)
                {
                    m_AudioSource.Stop();
                    m_AudioSource.clip = m_VRDissapearSounds[Random.Range(0, m_VRDissapearSounds.Length)];
                    m_AudioSource.Play();
                }
                
                m_DeadParticle.Play();
            }
            else
            {
                m_MaxPauseTime -= Time.deltaTime;
            }
            if (m_MaxPauseTime < 0)
            {
                m_AllowBodyClear = true;
            }
        }
    }

    

    public override void ResetAll()
    {
        base.ResetAll();
        m_PlayStarted = false;
        m_FloatingTime = 0;
        m_MaxPauseTime = Mathf.Max(m_DeadParticle.main.duration, 2f);
        m_DeadParticle.Stop();
        
    }

    public override GameObject Clone(Vector3 at, Quaternion to = default)
    {
        GameObject go = base.Clone(at, to);
        EnemySoldier_VR profile = GetComponent<EnemySoldier_VR>();
        var main = profile.m_DeadParticle.main;
        
        Color color = profile.m_Skin.sharedMaterial.GetColor("_FrontColor");
        color.a = m_DeadParticle.main.startColor.color.a;
        main.startColor = color;
        return go;
    }
}
