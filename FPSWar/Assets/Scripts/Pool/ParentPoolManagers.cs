using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentPoolManagers:MonoBehaviour
{
    public static ParentPoolManagers Instance{get;private set;}
    private Dictionary<TypeObjectPoolManager,ObjectPoolManager> objManagers = new Dictionary<TypeObjectPoolManager, ObjectPoolManager>();

    private void Start() {
       if(Instance==null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        FindObjManagerChild();
    }
    private void FindObjManagerChild()
    {
        foreach(Transform child in transform)
        {
            if(child.TryGetComponent<ObjectPoolManager>(out ObjectPoolManager objManager))
            {
                objManagers.Add(objManager.TypeObjectPoolManager,objManager);
            }
        }
    }
    public ObjectPoolManager GetObjPoolManager(TypeObjectPoolManager type)
    {
        return objManagers[type];
    }

}
