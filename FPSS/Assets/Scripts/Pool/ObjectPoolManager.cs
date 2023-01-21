using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeObjectPoolManager
{
    PROJECTILE_FPS,
    PROJECTILE_TPP,
    ROCKET_FPS,
    ROCKET_TPP,
    SHELL_BULLET
}
public interface IObjectPool
{
    bool IsReadyForTakeOut();
    void ReturnToNewState(Vector3 position, Vector3 rotation);
    void SetActive();
    
}
public interface IProjectilePool:IObjectPool
{
    void SetBeforeShoot(PlayerController owner, int damage,RaycastHit hit);
}
public class ObjectPoolManager : MonoBehaviour
{
    [field:SerializeField] public TypeObjectPoolManager TypeObjectPoolManager{get;private set;}
    private List<IObjectPool> objs = new List<IObjectPool>();

    public void AddObjPool(IObjectPool obj)
    {
        if(objs.Contains(obj)){return;}
        objs.Add(obj);
    }
    public IObjectPool GetReadyObject()
    {
        foreach(var obj in objs)
        {
            if(obj.IsReadyForTakeOut())
            {
                return obj;
            }
        }
        return null;
    }

}
