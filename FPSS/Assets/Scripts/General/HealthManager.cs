using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;
public class HealthManager : NetworkBehaviour
{
    public event Action OnDie;
    public event Action OnNearlyDie;
    public event Action OnStronger;
    public event Action OnTakeDamage;
    [SerializeField] private List<Health> healths;
    [SerializeField] private int maxHealth = 200;
    [SerializeField] private int lowHealth = 10;
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
        OnNearlyDie+=UIManager.Instance.TriggerNearlyDieUI;
        OnStronger+=UIManager.Instance.TriggerStopNearlyDieUI;
        OnDie+=UIManager.Instance.TriggerStopNearlyDieUI;
    }
    private void OnDestroy()
    {
        UnsubcribeHealth();
        if(!isOwned){return;}
        OnTakeDamage-=UIManager.Instance.TriggerBloodOverlay;
        OnNearlyDie-=UIManager.Instance.TriggerNearlyDieUI;
        OnStronger-=UIManager.Instance.TriggerStopNearlyDieUI;
        OnDie-=UIManager.Instance.TriggerStopNearlyDieUI;
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
    public void TakeDamage(int amount,Transform attacker)
    {
        if(isOwned)
        {
            TriggerDamageIndicator(attacker);
        }
        if (!isServer) { return; }
        ServerTakeDamage(amount);
    }
    private void TriggerDamageIndicator(Transform attacker)
    {
        if(attacker==null){return;}
        DISystem.Instance.CreateIndicator(attacker);
        // if(!DISystem.Instance.CheckIfObjectInsight(attacker))
        // {
            
        // }
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
        if(currentHealth<lowHealth&&currentHealth>0)
        {
            OnNearlyDie?.Invoke();
        }
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
