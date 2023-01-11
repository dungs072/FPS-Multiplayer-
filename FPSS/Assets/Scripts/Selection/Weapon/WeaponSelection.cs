using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelection : MonoBehaviour
{
    [Header("Scope")]
    [SerializeField] private ScopeSelection[] scopePackUI;
    [SerializeField] private GameObject[] scopePack;
    [SerializeField] private string nameWeapon;

    private WeaponAdjustment weaponAdjustment;
    private void Start()
    {
        string name = "FPSWeapons/" + nameWeapon;
        weaponAdjustment = Resources.Load<WeaponAdjustment>(name);
    }
    public void DisplayScopePackUI()
    {
        for (int i = 0; i < scopePackUI.Length; i++)
        {
            ScopeSelection itemInstance = Instantiate(scopePackUI[i], UISelection.Instance.ScopePack);
            RegisterScopeForWeapon(itemInstance, i);
        }
    }
    private void RegisterScopeForWeapon(ScopeSelection scope, int index)
    {
        if (index >= scopePack.Length) { return; }
        scope.OnSelect += scopePack[index].SetActive;
        scope.OnDeactive += DeactiveAllScopePack;
        scope.OnToggleScope+=weaponAdjustment.ToggleScope;
        scope.Index = index;
    }
    private void DeactiveAllScopePack()
    {
        foreach (var item in scopePack)
        {
            item.SetActive(false);
        }
    }


}
