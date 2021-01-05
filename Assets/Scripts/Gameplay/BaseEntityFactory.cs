using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//deprecated


//Manifest是为了节省一些继承的接口数量，
//子类直接复写自己的生成方法，在其中决定
//如何使用manifest及其自定义的子类。
[System.Obsolete("using prototype for enemygen instead.")]
public abstract class BaseEntityFactory : MonoBehaviour
{
    [SerializeField]
    protected GameObject[] products;
    
    protected int progress = 0;
    

    protected BaseFactoryManifest manifest;

    private void Awake()
    {
        manifest = new BaseFactoryManifest();
    }
    //立刻生产一个单位
    public abstract GameObject GenEntity();

    //部分生产，可以控制生产一个单位的速度。
    public virtual GameObject MakeProgress() 
    {
        progress += manifest.genSpeed;
        if(manifest.maxProgress < progress)
        {
            progress -= manifest.maxProgress;
            return GenEntity();
        }
        else
        {
            return null;
        }
    }

    public void InstallManifest(BaseFactoryManifest newManifest)
    {
        manifest = newManifest;
    }

    
}
