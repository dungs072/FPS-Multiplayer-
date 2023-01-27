using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class HandleExitGame : NetworkBehaviour
{
    private void Start() {
        UIManager.OnExitMatch+=ExitMatch;
    }
    private void OnDestroy() {
        UIManager.OnExitMatch-=ExitMatch;
    }

    private void ExitMatch()
    {
        if(isClientOnly)
        {
            NetworkManager.singleton.StopClient();
        }
        else
        {
            NetworkManager.singleton.StopHost();
        }
    }
}
