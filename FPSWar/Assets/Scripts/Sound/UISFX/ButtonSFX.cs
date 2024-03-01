using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ButtonSFX : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private bool changeExistSprite = true;
    private Image buttonImage;
    private void Start() {
        buttonImage = GetComponent<Image>();
    }
    [SerializeField] private AssetsSFX assetsSFX;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(changeExistSprite)
        {
            buttonImage.sprite = assetsSFX.PressButton;
        }
        audioSource.PlayOneShot(assetsSFX.ClickSound);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(changeExistSprite)
        {
            buttonImage.sprite = assetsSFX.HoverButton;
        }
        
        audioSource.PlayOneShot(assetsSFX.HoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(changeExistSprite)
        {
            buttonImage.sprite = assetsSFX.NormalButton;
        }
        
    }

    
}
