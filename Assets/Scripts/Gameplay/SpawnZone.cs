using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 在一个区域生成物体，在同一个地点生成不同物体的需求
通过向factories加入更多factory解决，不同区域形状的需求
通过给委托安装不同的委托函数解决，这样免得新建一堆文件，
但是有些别的麻烦发生了，比如drawGizmo会有问题，因为
SpawnZoneMethod不应是MonoBehavior，无法在内部应用
OnDrawGizmo，如果在SpawnZone中直接写，那么SpawnZone又
需要获知生成随机点的具体实现，破坏了整个结构，并且如果
想要扩展更多的随机点生成方式，只能直接扩写SpawnZoneMethod，
除非让SpawnZoneMethod不保持静态，或者应用单例模式。
 

将来需要改造成支持预先设计好的生成队列，打算写一个解释器
读入预先的json来解决这个问题。

好的我们现在有AssetsLoader了。
 */
public class SpawnZone : MonoBehaviour
{
    
    
    [SerializeField]
    private SpawnZoneDelegate.Type m_AreaType;
    [SerializeField]
    private bool m_SnapGround = false;
    [SerializeField]
    private bool m_OnSurface = false;
    [SerializeField]
    private bool m_BuiltInfactory = false;
    [SerializeField]
    private bool m_PartialGeneration = true;
    SpawnZoneDelegate.getRandomPointGenerator m_Generator;
    SpawnZoneDelegate.drawGizmo m_DrawGizmo;


    [SerializeField]
    protected int m_SpawnLimit = 20;

    [SerializeField]
    protected List<GameObject> g_Products;

    private List<GameObject> m_Active;

    private void Awake()
    {
        InitArea();
    }
    // Start is called before the first frame update
    void Start()
    {
        m_Active = new List<GameObject>();
        CharacterManager.instance.AddUnitReference(this);
    }

    // Update is called once per frame
    private int timer = 0; 
    void Update()
    {
        timer++;
        if (timer > 60)
        {
            Clearlist();
            timer = 0;
        }
        if (CanGen())
        {
            
            GenEntity();//
        }
    }

    protected void InitArea()
    {
        m_Generator = SpawnZoneMethod.NewGenerator(m_AreaType);
        //drawGizmo = SpawnZoneMethod.GetDrawType(areaType);
        if (g_Products == null) { g_Products = new List<GameObject>(); }
    }

    protected Vector3 Random3DPoint()
    {
        Vector3 p = transform.TransformPoint(m_Generator(m_OnSurface));
        if (m_SnapGround)
        {
            if (Physics.Raycast(transform.position, -Vector3.up,out RaycastHit hitInfo))
            {
                
                p.y = hitInfo.point.y + 0.05f;
                
            }
        }
        return p;
    }
    protected virtual void Clearlist()
    {
        m_Active.RemoveAll(i => i == null);
    } 
    protected virtual void GenEntity()
    {
        AddToList(Instantiate(g_Products[Random.Range(0, g_Products.Count)], Random3DPoint(), new Quaternion()));
        
    }

    protected virtual void GenEntity(int i)
    {
        AddToList(Instantiate(g_Products[i % g_Products.Count], Random3DPoint(), new Quaternion()));
    }

    protected virtual void AddToList(GameObject go)
    {
        m_Active.Add(go);
    }

    protected virtual bool CanGen()
    {
        if (autoGen)
        {
            return Random.Range(0, 30) > 27f && m_Active.Count < m_SpawnLimit;
        }
        else
        {
            if(Random.Range(0, 30) > 27f && m_Active.Count < m_SpawnLimit && requiredGenNum > 0)
            {
                requiredGenNum--;
                return true;
            }
            return false;
        }
    }
    [SerializeField]
    protected bool autoGen = true;
    public void SetAutoGen(bool set)
    {
        autoGen = set;
    }
    protected int requiredGenNum;
    public virtual void Gen(int num)
    {
        requiredGenNum = num;
    }

    public virtual void GenBrust(int num)
    {

    }
}
