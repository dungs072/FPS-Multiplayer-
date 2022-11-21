using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkPlayerManager : NetworkBehaviour
{
    [SerializeField] GameObject[] nonLocalObjects;
    [SerializeField] Camera[] cameras;
    [SerializeField] AudioListener audioListener;

    private void Start()
    {
        if(!isOwned)
        {
            foreach(var obj in nonLocalObjects)
            {
                obj.gameObject.SetActive(false);
            }
            foreach(var camera in cameras)
            {
                camera.enabled = false;
            }
            audioListener.enabled = false;
        }
    }
}
