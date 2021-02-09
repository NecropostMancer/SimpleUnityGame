using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathFinder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public NavMeshPath calc(Vector3 targetPosition, NavMeshAgent agent)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(targetPosition,path);
        return path;
    }
}
