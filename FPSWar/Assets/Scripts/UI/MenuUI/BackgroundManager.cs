using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite waitingBackground;
    [SerializeField] private Sprite mainBackground;
    public void ChangeBackgroundImage(BGImageType type)
    {
        if (type == BGImageType.MainBG)
        {
            backgroundImage.sprite = mainBackground;
        }
        else if (type == BGImageType.WaitingBG)
        {
            backgroundImage.sprite = waitingBackground;
        }
    }
}
public enum BGImageType
{
    MainBG,
    WaitingBG
}
