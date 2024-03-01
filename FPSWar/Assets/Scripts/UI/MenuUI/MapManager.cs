using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapManager : MonoBehaviour
{
    [SerializeField] private List<MapChoice> mapchoices;
    [SerializeField] private Image currentMapSelection;
    [SerializeField] private GameObject MapChoicePage;
    [SerializeField] private TMP_Text mapNameText;
    [Header("Lobby")]
    [SerializeField] private Image lobbyMap;
    [SerializeField] private TMP_Text lobbyMapName;
    private int currentMapIndex;
    private void Start() {
        ResetMap();
    }
    public void OnMapClick(MapChoice mapchoice)
    {
        for(int i =0;i<mapchoices.Count;i++)
        {
            if(mapchoices[i]==mapchoice)
            {
                mapchoices[i].ToggleSelectionRing(true);
                LobbyMenu.CurrentMapName = mapchoices[i].MapNameInSystem; 
                currentMapIndex = i;
            }
            else
            {
                mapchoices[i].ToggleSelectionRing(false);
            }
        }
    }    
    public void OnChooseCurrentMap()
    {
        ChangeSelectionMapImage(currentMapIndex);
        //NetworkClient.connection.identity.GetComponent<PlayerController>().CmdChangeSelectionMap(currentMapIndex);
        MapChoicePage.SetActive(false);
    }
    // for host
    public void ChangeSelectionMapImage(Sprite sprite,string mapName)
    {
        currentMapSelection.sprite = sprite;
        lobbyMap.sprite = sprite;
        mapNameText.text = mapName;
        lobbyMapName.text = mapName;
    }
    // for client
    public void ChangeSelectionMapImage(int mapIndex)
    {
        var map = mapchoices[mapIndex];
        ChangeSelectionMapImage(map.CurrentMapImage,map.GetMapNameDisplay());
    }
    public void ResetMap()
    {
        currentMapIndex = -1;
        currentMapSelection.sprite = null;
    }
    public bool IsEmptyMap()
    {
        return currentMapIndex==-1;
    }
    public string GetMapName()
    {
        return mapchoices[currentMapIndex].GetMapNameDisplay();
    }
    public int GetCurrentMapIndex()
    {
        return currentMapIndex;
    }
}
