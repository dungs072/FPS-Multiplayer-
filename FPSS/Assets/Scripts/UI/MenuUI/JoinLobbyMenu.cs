using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] private GameObject joinTable;
    [SerializeField] private TMP_InputField addressInput;
    [SerializeField] private Button joinButton;

    private void OnEnable() {
        MyNetworkManager.ClientOnConnected+=HandleClientConnected;
        MyNetworkManager.ClientOnDisconnected+=HandleClientDisconnected;
    }
    private void OnDisable() {
        MyNetworkManager.ClientOnConnected-=HandleClientConnected;
        MyNetworkManager.ClientOnDisconnected-=HandleClientDisconnected;
    }

    public void Join()
    {
        string address = addressInput.text;
        MyNetworkManager.singleton.networkAddress = address;
        MyNetworkManager.singleton.StartClient();
        joinButton.interactable = false;
    }
    private void HandleClientConnected()
    {
        joinTable.SetActive(false);
        joinButton.interactable = true;
    }
    private void HandleClientDisconnected()
    {
        joinTable.SetActive(true);
        joinButton.interactable = true;
    }
}
