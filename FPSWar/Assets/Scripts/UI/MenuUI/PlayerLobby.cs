using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerLobby : MonoBehaviour
{
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private GameObject readyIcon;
    public string GetPlayerName()
    {
        return playerName.text;
    }
    public void SetPlayerName(string name)
    {
        playerName.text = name;
    }
    public void ToggleReadyIcon(bool state)
    {
        readyIcon.SetActive(state);
    }
    public bool GetReadyState()
    {
        return readyIcon.activeSelf;
    }
}
