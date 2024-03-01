using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomInputManager : MonoBehaviour
{
    [SerializeField] private InputAttribute inputAttribute;
    public static CustomInputManager Instance { get; private set; }
    public InputAttribute Input{get{return inputAttribute;}}
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start() 
    {
        SettingManager.OnApplySettingChange+=inputAttribute.ApplyInput;
        SettingManager.OnSettingDefault+=inputAttribute.ResetToDefault;    
    }
    private void OnDestroy() 
    {
        SettingManager.OnApplySettingChange-=inputAttribute.ApplyInput;  
        SettingManager.OnSettingDefault-=inputAttribute.ResetToDefault;
    }
}
public class SettingChange
{
    public KeyCode reload;
    public KeyCode running;
    public KeyCode Crouch;
    public KeyCode Pickup;
    public KeyCode LeaningLeft;
    public KeyCode LeaningRight;
    public float MouseX;
    public float MouseY;
}
