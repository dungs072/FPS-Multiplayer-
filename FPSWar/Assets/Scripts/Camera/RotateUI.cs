using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateUI : MonoBehaviour
{
    private Camera lobbyCamera;
    private void LateUpdate() {
        if(lobbyCamera==null){return;}
        transform.LookAt(transform.position+lobbyCamera.transform.rotation*Vector3.forward,
                        lobbyCamera.transform.rotation*Vector3.up);
    }
    public void SetUpLobbyCamera(Camera camera)
    {
        lobbyCamera = camera;
    }
}
