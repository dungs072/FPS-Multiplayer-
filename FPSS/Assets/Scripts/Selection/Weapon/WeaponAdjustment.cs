using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAdjustment : MonoBehaviour
{
    [SerializeField] private GameObject[] scopes;

    public void ToggleScope(int index, bool state)
    {
        for (int i = 0; i < scopes.Length; i++)
        {
            scopes[i].SetActive(false);
        }
        if (state)
        {
            if(index>=scopes.Length){return;}
            scopes[index].SetActive(true);
        }
    }

}
