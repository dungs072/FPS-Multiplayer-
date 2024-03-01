using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class EventRegister : MonoBehaviour
{
    [SerializeField] private FirstPersonController fps;

    private void Start() {
        SettingManager.OnMouseXSenChange+=fps.SetRotatateXSensitivity;
        SettingManager.OnMouseYSenChange+=fps.SetRotateYSensitivity;
    }
    private void OnDestroy() {
        SettingManager.OnMouseXSenChange-=fps.SetRotatateXSensitivity;
        SettingManager.OnMouseYSenChange-=fps.SetRotateYSensitivity;
    }
}
