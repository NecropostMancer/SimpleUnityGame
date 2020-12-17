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
 */
public class SpawnZone : MonoBehaviour
{
    
    
    [SerializeField]
    SpawnZoneDelegate.type areaType;
    [SerializeField]
    bool snapGround = false;
    [SerializeField]
    bool onSurface = false;
    [SerializeField]
    bool builtInfactory = false;
    [SerializeField]
    bool partialGeneration = true;
    SpawnZoneDelegate.getRandomPointGenerator generator;
    SpawnZoneDelegate.drawGizmo drawGizmo;


    

    [SerializeField]
    List<GameObject> products;
    [SerializeField]
    List<BaseEntityFactory> factories;
    private void Awake()
    {
        InitArea();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CanGen())
        {
            
            GenEntity();
        }
    }

    private void OnDrawGizmos()
    {
        
    }

    protected void InitArea()
    {
        generator = SpawnZoneMethod.NewGenerator(areaType);
        //drawGizmo = SpawnZoneMethod.GetDrawType(areaType);
        if (products == null) { products = new List<GameObject>(); }
    }

    protected Vector3 Random3DPoint()
    {
        Vector3 p = transform.TransformPoint(generator(onSurface));
        if (snapGround)
        {
            
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, -Vector3.up,out hitInfo))
            {
                
                p.y = hitInfo.point.y;
                
            }
        }
        return p;
    }

    protected virtual void GenEntity()
    {
        if (builtInfactory)
        {
            Instantiate(products[Random.Range(0, products.Count)], Random3DPoint(), new Quaternion());
        }
        else
        {
            if (partialGeneration)
            {
                foreach(BaseEntityFactory factory in factories)
                {
                    GameObject obj = factory.MakeProgress();
                    if (obj)
                    {
                        Instantiate(obj, Random3DPoint(), new Quaternion());
                    }
                }
            }
        }
    }

    protected virtual bool CanGen()
    {
        return Random.Range(0, 30) > 27f;
    }
}
