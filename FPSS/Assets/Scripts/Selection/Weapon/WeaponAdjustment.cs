using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAdjustment : MonoBehaviour
{
    [SerializeField] private ScopeAttachment[] scopes;
    [Header("Some guns have iron sight")]
    [SerializeField] private GameObject ironSight;

    public ScopeAttachment ToggleScope(string nameScope, bool state)
    {
        ScopeAttachment scope = null;
        ToggleIronSight(true);
        for (int i = 0; i < scopes.Length; i++)
        {
            scopes[i].gameObject.SetActive(false);
            scopes[i].ToggleDistanceScope(false);
            if(scopes[i].ScopeInfor.Name==nameScope)
            {
                scopes[i].gameObject.SetActive(state);
                scopes[i].ToggleDistanceScope(true);
                ToggleIronSight(false);
                scope = scopes[i];
            }
        }
        return scope;
    }
    private void ToggleIronSight(bool state)
    {
        if(ironSight==null){return;}
        ironSight.SetActive(state);
    }
    public ScopeAttachment[] GetAllScope()
    {
       return scopes;
    }

}
