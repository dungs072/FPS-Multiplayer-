using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleShellBulletOut : MonoBehaviour
{
    [SerializeField] private ObjectPoolManager poolManager;
    [SerializeField] private ShellBullet shellBulletPrefab;
    [SerializeField] private Transform posSpawn;
    [SerializeField] private Transform ownParent;
    [SerializeField] private float force = 10f;


    public void CreateShellBullet()
    {
        IObjectPool objPool = poolManager.GetReadyObject();
        if (objPool == null)
        {
            ShellBullet shellBulletInstance = Instantiate(shellBulletPrefab, ownParent);
            shellBulletInstance.GetComponent<Rigidbody>().AddForce(shellBulletInstance.transform.right * force,
                                                                    ForceMode.Impulse);
            poolManager.AddObjPool(shellBulletInstance);
        }
        else
        {
            objPool.ReturnToNewState(posSpawn.position,Vector3.zero);
            objPool.SetActive();
        }

    }
}
