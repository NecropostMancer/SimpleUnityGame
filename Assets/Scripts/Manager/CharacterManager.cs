using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : Singleton<CharacterManager>
{
    private readonly Dictionary<int, BaseEnemy> m_EnemyRef = new Dictionary<int, BaseEnemy>();
    private readonly Dictionary<int, SpawnZone> m_SpawnRef = new Dictionary<int, SpawnZone>();

    private List<EnemySpawnZone> m_SoldierSpawnRef = new List<EnemySpawnZone>();
    private List<EnemySpawnZone> m_TankSpawnRef = new List<EnemySpawnZone>();
    private List<EnemySpawnZone> m_AirSpawnRef = new List<EnemySpawnZone>();

    private PlayerEntity m_PlayerEntity;
    [SerializeField]
    private EnemiesWave[] m_EasyWaves;
    [SerializeField]
    private EnemiesWave[] m_NormalWaves;
    [SerializeField]
    private EnemiesWave[] m_HardWaves;

    private EnemiesWave[] m_SelectedWaves;
    private List<EnemiesWave> m_WaveCopy = new List<EnemiesWave>();
    private int m_Req;
    private int m_Mode;

    private bool m_AllDone = true;

    public void AddUnitReference(BaseEnemy baseEnemy)
    {
        m_EnemyRef.Add(baseEnemy.GetInstanceID(), baseEnemy);
    }
    public void AddUnitReference(SpawnZone spawnZone)
    {
        m_SpawnRef.Add(spawnZone.GetInstanceID(), spawnZone);
        if(spawnZone.GetType() == typeof(EnemySpawnZone))
        {
            var sref = (EnemySpawnZone)spawnZone;
            if (sref.m_Tags.Contains(EnemySpawnZone.Tag.SOLDIER))
            {
                m_SoldierSpawnRef.Add(sref);
                m_SoldierSpawnRef.Sort((a,b)=> { return a.order - b.order; });
            }
            if (sref.m_Tags.Contains(EnemySpawnZone.Tag.TANK))
            {
                m_TankSpawnRef.Add(sref);
                m_TankSpawnRef.Sort((a, b) => { return a.order - b.order; });
            }
            if (sref.m_Tags.Contains(EnemySpawnZone.Tag.AIR))
            {
                m_AirSpawnRef.Add(sref);
                m_AirSpawnRef.Sort((a, b) => { return a.order - b.order; });
            }
        }
    }
    public void AddUnitReference(PlayerEntity playerEntity)
    {
        m_PlayerEntity = playerEntity;
    }
    public void RemoveUnitReference(BaseEnemy baseEnemy)
    {
        m_EnemyRef.Remove(baseEnemy.GetInstanceID());
        BattleManager.instance.InformKill();
    }
    public void RemoveUnitReference(int index)
    {
        m_EnemyRef.Remove(index);
    }
    public void AttackPlayer(float v)
    {
        if (!m_PlayerEntity.Damaged(v))
        {
            BattleManager.instance.InformPlayerDead();
        }
        Debug.Log(v);
    }
    public bool GetPlayerPos(out Vector3 pos)
    {
        if (m_PlayerEntity)
        {
            pos = m_PlayerEntity.transform.position;
            return true;
        }
        else
        {
            pos = new Vector3();
            return false;
        }
    }
    public void ResetHP()
    {
        m_PlayerEntity.ReFill();
    }
    public void StartRecord()
    {
        m_Req = BattleManager.instance.GetReq();
        m_Mode = BattleManager.instance.GetMode();
        if (m_Mode == 0)
        {
            if (m_Req >= m_EasyWaves.Length)
            {
                m_SelectedWaves = m_EasyWaves;
                if (m_Req >= m_NormalWaves.Length)
                {
                    m_SelectedWaves = m_NormalWaves;
                    if (m_Req >= m_HardWaves.Length)
                    {
                        m_SelectedWaves = m_HardWaves;
                    }
                }
            }
        }
        else
        {
            m_SelectedWaves = m_HardWaves;
        }
        if(m_SelectedWaves == null)
        {
            m_SelectedWaves = m_EasyWaves;
        }
        foreach (var wave in m_SelectedWaves)
        {
            m_WaveCopy.Add(wave.Clone());
        }

        m_AllDone = false;
        m_CurPeriod = WAVE_PERIOD.WAVE_RUN;
        BattleManager.instance.InformNextWaveCame();

    }
    public void ClearAll()
    {
        m_EnemyRef.Clear();
        m_SpawnRef.Clear();
        m_SoldierSpawnRef.Clear();
        m_TankSpawnRef.Clear();
        m_AirSpawnRef.Clear();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    float TimeSinceLastGen = 0;
    float RestTime = 0;
    public enum WAVE_PERIOD {FRESH_START,WAVE_END,WAVE_RUN,WAVE_PREPARE}
    private WAVE_PERIOD m_CurPeriod = WAVE_PERIOD.FRESH_START;

    // Update is called once per frame
    void Update()
    {
        
        if (m_AllDone) return;
        switch (m_CurPeriod)
        {
            case WAVE_PERIOD.WAVE_RUN:
                if (m_EnemyRef.Count < 3)
                {
                    TimeSinceLastGen += Time.deltaTime;
                    if (TimeSinceLastGen > 6 || (m_EnemyRef.Count == 0 && TimeSinceLastGen > 3))
                    {
                        SpawnNextSquad();
                        TimeSinceLastGen = 0;
                    }
                }
                break;
            case WAVE_PERIOD.WAVE_END:
                RestTime -= Time.deltaTime;
                
                if (RestTime < 0)
                {
                    m_CurPeriod = WAVE_PERIOD.WAVE_PREPARE;
                    BattleManager.instance.InformWaveEnd();
                    RestTime = 4;
                }
                break;
            case WAVE_PERIOD.WAVE_PREPARE:
                RestTime -= Time.deltaTime;
                if (RestTime < 0)
                {
                    m_CurPeriod = WAVE_PERIOD.WAVE_RUN;
                    BattleManager.instance.InformNextWaveCame();

                }
                

                break;
            default:
                
                break;
        }
        



    }


    private int SquadPtr;
    private int WavePtr;
    void SpawnNextSquad()
    {
        if(m_SpawnRef.Count == 0)
        {
            Debug.LogError("No Spawn here.");
            return;
        }
        if (SquadPtr >= m_WaveCopy[WavePtr].Squads.Count)
        {
            if (m_EnemyRef.Count != 0)
            {
                return; //if there are enemies alive, dont spawn next wave.
            }
            SquadPtr = 0;
            WavePtr++;
            //BattleManager.instance.InformWaveEnd();
            m_CurPeriod = WAVE_PERIOD.WAVE_END;

            RestTime = 2;
            TimeSinceLastGen = 0;
            
            if(WavePtr >= m_WaveCopy.Count)
            {
                if (m_Mode != 0)
                {
                    m_WaveCopy.Add(m_SelectedWaves[m_SelectedWaves.Length - 1].Clone());
                    SquadPtr = 0;
                    
                }
                else
                {
                    m_AllDone = true;
                    BattleManager.instance.InfromAllWaveEnd();
                    
                }
                
            }
            return;
        }
        
        //int UnitPtr;
        int cnt = Random.Range(3, 8);
        bool[] end = { false, false, false };
        if(m_SoldierSpawnRef.Count == 0) { end[0] = true; }
        if(m_TankSpawnRef.Count == 0) { end[1] = true; }
        if (m_AirSpawnRef.Count == 0) { end[2] = true; }
        foreach (var spawner in m_SoldierSpawnRef)
        {
            if (m_WaveCopy[WavePtr].Squads[SquadPtr].members[0] <= 0)
            {
                end[0] = true;
                break;
            }
            spawner.Gen(Mathf.Min(m_WaveCopy[WavePtr].Squads[SquadPtr].members[0],cnt));
            m_WaveCopy[WavePtr].Squads[SquadPtr].members[0] -= cnt;
            

        }
        cnt = Random.Range(1, 2);
        foreach (var spawner in m_TankSpawnRef)
        {
            if (m_WaveCopy[WavePtr].Squads[SquadPtr].members[1] <= 0)
            {
                end[1] = true;
                break;
            }
            spawner.Gen(Mathf.Min(m_WaveCopy[WavePtr].Squads[SquadPtr].members[1], cnt));
            m_WaveCopy[WavePtr].Squads[SquadPtr].members[1] -= cnt;
            

        }
        cnt = Random.Range(1, 3);
        foreach (var spawner in m_AirSpawnRef)
        {
            if (m_WaveCopy[WavePtr].Squads[SquadPtr].members[2] <= 0)
            {
                end[2] = true;
                break;
            }
            spawner.Gen(Mathf.Min(m_WaveCopy[WavePtr].Squads[SquadPtr].members[2], cnt));
            m_WaveCopy[WavePtr].Squads[SquadPtr].members[2] -= cnt;
            

        }

        if(end[0] && end[1] && end[2])
        {
            SquadPtr++;
        }

    }

    public void ForceStop()
    {
        m_AllDone = true;
    }
}
