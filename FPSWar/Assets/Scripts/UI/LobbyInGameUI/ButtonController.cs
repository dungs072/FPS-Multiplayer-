using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private Button button;
    public void SetInteractable(bool state)
    {
        button.interactable = state;
    }
}
