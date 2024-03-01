using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUI : MonoBehaviour
{
    [SerializeField] private GameObject tab;
    [SerializeField] private GameObject panel;
    [field:SerializeField] public SettingType SettingType{get;private set;}
    [SerializeField] private SettingManager settingManagerUI;

    public void ToggleSetting(bool state)
    {
        tab.SetActive(!state);
        panel.SetActive(state);
    }
    public void OnClickSetting()
    {
        settingManagerUI.OnSettingOptionClick(SettingType);
    }
}
