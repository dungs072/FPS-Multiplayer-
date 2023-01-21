using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    [SerializeField] private RectTransform crossHair;
    [SerializeField] private RectTransform shortCrossHair;
    [SerializeField] private float aimSize = 25f;
    [SerializeField] private float idleSize = 50f;
    [SerializeField] private float walkSize = 75f;
    [SerializeField] private float runJumpSize = 125f;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float shootSpeed = 1f;
    [SerializeField] private float shootSingleSpeed = 20f;
    [SerializeField] private float maxShootSize = 150;

    private RectTransform currentCrossHair;

    private float currentSize = 50f;
    private bool canExpandCrossHair = true;
    private PlayerController playerController;
    private void Start()
    {
        currentCrossHair = crossHair;
        MyNetworkManager myNetworkManager = (MyNetworkManager)NetworkManager.singleton;
        myNetworkManager.OnAddPlayers += FindPlayer;
    }
    private void OnDestroy()
    {
        MyNetworkManager myNetworkManager = (MyNetworkManager)NetworkManager.singleton;
        if (myNetworkManager == null) { return; }
        myNetworkManager.OnAddPlayers -= FindPlayer;
    }
    public void ChangeCrossHair(ItemType type)
    {
        if(type==ItemType.ShotGun)
        {
            shortCrossHair.gameObject.SetActive(true);
            crossHair.gameObject.SetActive(false);
            currentCrossHair = shortCrossHair;
        }
        else
        {
            shortCrossHair.gameObject.SetActive(false);
            crossHair.gameObject.SetActive(true);
            currentCrossHair = crossHair;
        }
    }
    public void FindPlayer()
    {
        if (playerController != null) { return; }
        MyNetworkManager myNetworkManager = (MyNetworkManager)NetworkManager.singleton;
        foreach (var player in myNetworkManager.PlayersAuthority)
        {
            if (player.isOwned)
            {
                playerController = player;
                break;
            }
        }
    }
    private void Update()
    {
        if (playerController == null) { return; }
        if (playerController.IsAiming)
        {
            currentSize = Mathf.Lerp(currentSize, aimSize, Time.deltaTime * speed);
        }
        else if (playerController.IsAttacking&&canExpandCrossHair)
        {
            float spd = 0;
            if (playerController.CurrentWeaponIsSingleFire())
            {
                spd = shootSingleSpeed;
                if (currentSize == maxShootSize)
                {
                    canExpandCrossHair = false;
                    return;
                }
            }
            else
            {
                spd = shootSpeed;
            }

            currentSize = Mathf.Lerp(currentSize, maxShootSize, Time.deltaTime * spd);
        }
        else if(!canExpandCrossHair)
        {
            currentSize = Mathf.Lerp(currentSize, idleSize, Time.deltaTime * (shootSpeed+5f));
            if(currentSize<idleSize+10f&&!playerController.IsAttacking)
            {
                canExpandCrossHair = true;
            }
        }   
        else if (playerController.IsWalking)
        {
            currentSize = Mathf.Lerp(currentSize, walkSize, Time.deltaTime * speed);
        }
        else if (playerController.IsRunning)
        {
            currentSize = Mathf.Lerp(currentSize, runJumpSize, Time.deltaTime * speed);
        }
        else
        {
            currentSize = Mathf.Lerp(currentSize, idleSize, Time.deltaTime * speed);
        }
        currentCrossHair.sizeDelta = new Vector2(currentSize, currentSize);
        
    }
}

