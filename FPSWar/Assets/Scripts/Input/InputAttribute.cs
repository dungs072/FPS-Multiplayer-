
using UnityEngine;

[CreateAssetMenu(fileName = "Input Attribute", menuName = "CustomInput")]
public class InputAttribute : ScriptableObject
{
    [Header("Default")]
    [SerializeField] private KeyCode reloadingKeyDefault = KeyCode.R;
    [SerializeField] private KeyCode crouchKeyDefault = KeyCode.C;
    [SerializeField] private KeyCode pickUpKeyDefault = KeyCode.F;
    [SerializeField] private KeyCode leaningLeftKeyDefault = KeyCode.Q;
    [SerializeField] private KeyCode leaningRightKeyDefault = KeyCode.E;
    [SerializeField] private KeyCode runningKeyDefault = KeyCode.LeftShift;
    [SerializeField] private float mouseXDefault = 2;
    [SerializeField] private float mouseYDefault = 2;
    [Header("Player setting")]
    [SerializeField] private KeyCode reloadingKey = KeyCode.R;
    [SerializeField] private KeyCode crouchKey = KeyCode.C;
    [SerializeField] private KeyCode pickUpKey = KeyCode.F;
    [SerializeField] private KeyCode leaningLeftKey = KeyCode.Q;
    [SerializeField] private KeyCode leaningRightKey = KeyCode.E;
    [SerializeField] private KeyCode runningKey = KeyCode.LeftShift;
    [Header("Mouse sensitivity")]
    [SerializeField] private float mouseX = 2;
    [SerializeField] private float mouseY = 2;


    public KeyCode ReloadingKeyCode
    {
        get
        {
            return reloadingKey;
        }
        set
        {
            reloadingKey = value;
        }
    }
    public KeyCode CrouchKeyCode
    {
        get
        {
            return crouchKey;
        }
        set
        {
            crouchKey = value;
        }
    }
    public KeyCode PickUpKeyCode
    {
        get
        {
            return pickUpKey;
        }
        set
        {
            pickUpKey = value;
        }
    }
    public KeyCode LeaningLeftKeyCode
    {
        get
        {
            return leaningLeftKey;
        }
        set
        {
            leaningLeftKey = value;
        }
    }
    public KeyCode LeaningRightKeyCode
    {
        get
        {
            return leaningRightKey;
        }
        set
        {
            leaningRightKey = value;
        }
    }
    public KeyCode RunningKeyCode
    {
        get
        {
            return runningKey;
        }
        set
        {
            runningKey = value;
        }
    }
    public float MouseX
    {
        get
        {
            return mouseX;
        }
    }
    public float MouseY
    {
        get
        {
            return mouseY;
        }
    }

    public KeyCode ReloadingKeyCodeDefault
    {
        get
        {
            return reloadingKeyDefault;
        }
        set
        {
            reloadingKeyDefault = value;
        }
    }
    public KeyCode CrouchKeyCodeDefault
    {
        get
        {
            return crouchKeyDefault;
        }
        set
        {
            crouchKeyDefault = value;
        }
    }
    public KeyCode PickUpKeyCodeDefault
    {
        get
        {
            return pickUpKeyDefault;
        }
        set
        {
            pickUpKeyDefault = value;
        }
    }
    public KeyCode LeaningLeftKeyCodeDefault
    {
        get
        {
            return leaningLeftKeyDefault;
        }
        set
        {
            leaningLeftKeyDefault = value;
        }
    }
    public KeyCode LeaningRightKeyCodeDefault
    {
        get
        {
            return leaningRightKeyDefault;
        }
        set
        {
            leaningRightKeyDefault = value;
        }
    }
    public KeyCode RunningKeyCodeDefault
    {
        get
        {
            return runningKeyDefault;
        }
        set
        {
            runningKeyDefault = value;
        }
    }
    public float MouseXDefault
    {
        get
        {
            return mouseXDefault;
        }
    }
    public float MouseYDefault
    {
        get
        {
            return mouseYDefault;
        }
    }
    public void ResetToDefault()
    {
        reloadingKey = reloadingKeyDefault;
        crouchKey = crouchKeyDefault;
        pickUpKey = pickUpKeyDefault;
        leaningLeftKey = leaningLeftKeyDefault;
        leaningRightKey = leaningRightKeyDefault;
        runningKey = runningKeyDefault;
        mouseX = mouseXDefault;
        mouseY = mouseYDefault;
    }
    public void ApplyInput(SettingChange settingChange)
    {
        reloadingKey = settingChange.reload;
        //runningKey = keyChange.running;
        crouchKey = settingChange.Crouch;
        pickUpKey = settingChange.Pickup;
        leaningLeftKey = settingChange.LeaningLeft;
        leaningRightKey = settingChange.LeaningRight;
        mouseX = settingChange.MouseX;
        mouseY = settingChange.MouseY;

    }
}
