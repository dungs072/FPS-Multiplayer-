using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;
public class HealthManager : NetworkBehaviour
{
    public event Action OnDie;
    public event Action OnTakeDamage;
    [SerializeField] private List<Health> healths;
    [SerializeField] private int maxHealth = 200;

    [SyncVar(hook = nameof(OnChangeCurrentHealth))]
    private int currentHealth;

    [SyncVar(hook = nameof(OnHandleDie))]
    private bool isDie = false;
    public bool IsDie { get { return isDie; } }

    private void Start()
    {
        currentHealth = maxHealth;
        SubscribeHealth();
    }
    public override void OnStartAuthority()
    {
        OnTakeDamage+=UIManager.Instance.TriggerBloodOverlay;
    }
    private void OnDestroy()
    {
        UnsubcribeHealth();
        if(!isOwned){return;}
        OnTakeDamage-=UIManager.Instance.TriggerBloodOverlay;
    }
    private void SubscribeHealth()
    {
        foreach (var health in healths)
        {
            health.OnTakeDamage += TakeDamage;
        }
    }
    private void UnsubcribeHealth()
    {
        foreach (var health in healths)
        {
            health.OnTakeDamage -= TakeDamage;
        }
    }
    public void TakeDamage(int amount)
    {
        if (!isServer) { return; }
        ServerTakeDamage(amount);
    }
    [Server]
    private void ServerTakeDamage(int amount)
    {
        if (isDie) { return; }
        currentHealth = Mathf.Max(currentHealth - amount, 0);
        print(gameObject.name +": "+ currentHealth);
        if (currentHealth == 0)
        {
            isDie = true;
            OnDie?.Invoke();
        }
    }
    private void OnHandleDie(bool oldState, bool newState)
    {
        if(isServer){return;}
        OnDie?.Invoke();
    }
    private void OnChangeCurrentHealth(int oldValue,int newValue)
    {
        if(!isOwned){return;}
        if(oldValue<newValue){return;}
        OnTakeDamage?.Invoke();
    }
    [Command]
    private void CmdIsDie(bool state)
    {
        isDie = state;
    }
    [Command(requiresAuthority = false)]//pain way
    private void CmdSetCurrentHealth(int amount)
    {
        if (isDie) { return; }
        currentHealth = amount;
        if (currentHealth == 0)
        {
            isDie = true;
        }
    }
}
