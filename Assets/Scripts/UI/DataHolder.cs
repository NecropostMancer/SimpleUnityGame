using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
public class DataHolder : MonoBehaviour
{
    private Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
    public void AddField(string key, object data)
    {
        keyValuePairs.Add(key, data);
    }
    public T GetField<T>(string key)
    {
        return (T)keyValuePairs[key];
    }
    public object GetField(string key)
    {
        return keyValuePairs[key];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
