using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class SettingManager : MonoBehaviour
{
    public static event Action<SettingChange> OnApplySettingChange;
    public static event Action OnSettingDefault;
    public static event Action<float> OnMouseXSenChange;
    public static event Action<float> OnMouseYSenChange;
    [SerializeField] private List<SettingUI> settings;
    [Header("Control")]
    [SerializeField] private Slider mouseXSlider;
    [SerializeField] private Slider mouseYSlider;
    [Header("KeyChange")]
    [SerializeField] private TMP_InputField reloadInput;
    [SerializeField] private TMP_InputField crouchInput;
    [SerializeField] private TMP_InputField pickupInput;
    [SerializeField] private TMP_InputField leaningLeftInput;
    [SerializeField] private TMP_InputField leaningRightInput;
    [Header("Invoking")]
    [SerializeField] private ObjectInvoking objectInvoking;
    [Header("Sound")]
    [SerializeField] private Slider soundSlider;

    private void Start()
    {
        OnSettingOptionClick(SettingType.Control);
        objectInvoking.OnEnableEvent += UpdateSettingValue;
    }
    private void OnDestroy()
    {
        objectInvoking.OnEnableEvent -= UpdateSettingValue;
    }

    public void OnSettingOptionClick(SettingType settingType)
    {
        foreach (var setting in settings)
        {
            setting.ToggleSetting(setting.SettingType == settingType);
        }
    }
    public void OnMouseXSliderChange()
    {
        OnMouseXSenChange?.Invoke(mouseXSlider.value);
    }
    public void OnMouseYSliderChange()
    {
        OnMouseYSenChange?.Invoke(mouseYSlider.value);
    }
    public void OnSetDefaultClick()
    {
        var inputManager = CustomInputManager.Instance;
        OnSettingDefault?.Invoke();
        OnMouseXSenChange?.Invoke(inputManager.Input.MouseXDefault);
        OnMouseYSenChange?.Invoke(inputManager.Input.MouseYDefault);
        mouseXSlider.value = inputManager.Input.MouseXDefault;
        mouseYSlider.value = inputManager.Input.MouseYDefault;
    }
    private void UpdateSettingValue()
    {
        var inputManager = CustomInputManager.Instance;
        reloadInput.text = inputManager.Input.ReloadingKeyCode.ToString();
        crouchInput.text = inputManager.Input.CrouchKeyCode.ToString();
        pickupInput.text = inputManager.Input.PickUpKeyCode.ToString();
        leaningLeftInput.text = inputManager.Input.LeaningLeftKeyCode.ToString();
        leaningRightInput.text = inputManager.Input.LeaningRightKeyCode.ToString();
        mouseXSlider.value = inputManager.Input.MouseX;
        mouseYSlider.value = inputManager.Input.MouseY;
    }

    public void OnApplyClick()
    {
        var settingChange = new SettingChange();
        if (Enum.TryParse(reloadInput.text.ToUpper(), out KeyCode reloadKey))
        {
            settingChange.reload = reloadKey;
        }
        if (Enum.TryParse(crouchInput.text.ToUpper(), out KeyCode crouchKey))
        {
            settingChange.Crouch = crouchKey;
        }
        if (Enum.TryParse(pickupInput.text.ToUpper(), out KeyCode pickupKey))
        {
            settingChange.Pickup = pickupKey;
        }
        if (Enum.TryParse(leaningLeftInput.text.ToUpper(), out KeyCode leaningLeftKey))
        {
            settingChange.LeaningLeft = leaningLeftKey;
        }
        if (Enum.TryParse(leaningRightInput.text.ToUpper(), out KeyCode leaningRightKey))
        {
            settingChange.LeaningRight = leaningRightKey;
        }
        settingChange.MouseX = mouseXSlider.value;
        settingChange.MouseY = mouseYSlider.value;
        OnApplySettingChange?.Invoke(settingChange);
    }
    public void SetVolume()
    {
        AudioListener.volume = soundSlider.value;
    }

}

public enum SettingType
{
    Control,
    Sound
}
