using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAdjustment : MonoBehaviour
{
    [SerializeField] private ScopeAttachment[] scopes;

    public ScopeAttachment ToggleScope(string nameScope, bool state)
    {
        for (int i = 0; i < scopes.Length; i++)
        {
            scopes[i].gameObject.SetActive(false);
            if(scopes[i].ScopeInfor.Name==nameScope)
            {
                scopes[i].gameObject.SetActive(state);
                return scopes[i];
            }
        }
        return null;
    }
    public ScopeAttachment[] GetAllScope()
    {
       return scopes;
    }

}
