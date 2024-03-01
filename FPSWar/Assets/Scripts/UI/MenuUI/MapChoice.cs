using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class MapChoice : MonoBehaviour
{
    [SerializeField] private GameObject selectionRing;
    [field:SerializeField] public string MapNameInSystem{get;private set;}
    [field:SerializeField] public Sprite CurrentMapImage{get;private set;}
    [field:SerializeField] public TMP_Text mapNameDisplayText{get;private set;}
    [field:SerializeField] public Image mapImageDisplay{get;private set;}
    public void ToggleSelectionRing(bool state)
    {
        selectionRing.SetActive(state);
    }
    public string GetMapNameDisplay()
    {
        return mapNameDisplayText.text;
    }
    public Sprite GetMapImageDisplay()
    {
        return mapImageDisplay.sprite;
    }
}
