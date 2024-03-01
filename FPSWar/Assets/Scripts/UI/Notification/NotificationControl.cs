using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NotificationControl : MonoBehaviour
{
    [SerializeField] private TMP_Text title;

    public void SetText(string text)
    {
        title.text = text;
    }
    public void PressOkButton()
    {
        gameObject.SetActive(false);
    }
}
