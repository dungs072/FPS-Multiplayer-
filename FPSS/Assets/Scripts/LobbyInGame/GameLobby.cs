using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class GameLobby : NetworkBehaviour
{

    public static GameLobby Instance{get;private set;}
    [SerializeField] private Transform[] lobbyPoints;

    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
