using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
public class MyNetworkManager : NetworkManager
{
    public event Action OnAddPlayers;
    public List<PlayerController> Players{get;} = new List<PlayerController>();

    public void AddPlayers(PlayerController playerController)
    {
        Players.Add(playerController);
        OnAddPlayers.Invoke();
    }
}
