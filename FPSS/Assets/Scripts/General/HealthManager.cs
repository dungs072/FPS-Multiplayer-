using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;
public class HealthManager : NetworkBehaviour
{
    public static event Action<TeamName, int> OnIncreasingScore;
    public event Action OnDie;
    public event Action OnNearlyDie;
    public event Action OnTakeDamage;
    public event Action OnRescuing;
    public event Action OnNormal;
    public event Action<float> OnChangeHealthBar;
    [SerializeField] private List<Health> healths;
    [SerializeField] private int maxHealth = 200;
    [SerializeField] private int lowHealth = 10;
    [Header("For healing")]
    [SerializeField] private float timeToHeal = 7f;
    [SerializeField] private int increaseHealthAmount = 10;
    [Header("Camera")]
    [SerializeField] private Camera fpsCamera;
    [Header("Revise")]
    [SerializeField] private float timeToImmune = 5f;
    private bool canTakeDamage = true;
    [SyncVar(hook = nameof(OnChangeCurrentHealth))]
    private int currentHealth;

    [SyncVar(hook = nameof(OnHandleDie))]
    private bool isDie = false;
    public bool IsDie { get { return isDie; } }

    private Coroutine healCoroutine;

    private void Start()
    {
        currentHealth = maxHealth;
        SubscribeHealth();
    }
    public override void OnStartAuthority()
    {
        OnTakeDamage += UIManager.Instance.TriggerBloodOverlay;
        OnNearlyDie += UIManager.Instance.TriggerNearlyDieUI;
        OnRescuing += UIManager.Instance.TriggerStopNearlyDieUI;
        OnDie += UIManager.Instance.TriggerStopNearlyDieUI;
        OnChangeHealthBar += UIManager.Instance.ChangeHealthBar;
    }
    private void OnDestroy()
    {
        UnsubcribeHealth();
        if (!isOwned) { return; }
        OnTakeDamage -= UIManager.Instance.TriggerBloodOverlay;
        OnNearlyDie -= UIManager.Instance.TriggerNearlyDieUI;
        OnRescuing -= UIManager.Instance.TriggerStopNearlyDieUI;
        OnDie -= UIManager.Instance.TriggerStopNearlyDieUI;
        OnChangeHealthBar -= UIManager.Instance.ChangeHealthBar;
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
    public void Respawn()
    {
        if (!isOwned) { return; }
        GetComponent<Team>().ReturnToLobbyPosition();
        isDie = false;
        CmdIsDie(false);
        CmdSetCurrentHealth(maxHealth);
        OnChangeHealthBar.Invoke(1f);
        OnNormal?.Invoke();
        if (healCoroutine != null) { StopCoroutine(healCoroutine); }
    }
    public void TakeDamage(int amount, bool isHead, Transform attackingOwner, Transform attacker)
    {
        if (!canTakeDamage) { return; }
        if (isOwned)
        {
            if (attacker != null&&!isDie)
            {
                TriggerDamageIndicator(attacker);
            }
        }
        string nameKiller = GetNameKiller(attackingOwner);
        if (attackingOwner.TryGetComponent<NetworkIdentity>(out NetworkIdentity identity))
        {
            if (identity.isOwned)
            {
                if (currentHealth - amount <= 0 && !isDie)
                {
                    UIManager.Instance.TriggerScoreRewardUI(isHead);
                }
                currentHealth = Mathf.Max(currentHealth - amount, 0);
                CmdTakeDamage(amount, nameKiller);
            }
        }
        else
        {
            if (isServer)
            {
                ServerTakeDamage(amount, nameKiller);
            }
        }
    }

    private static string GetNameKiller(Transform attackingOwner)
    {
        string nameKiller = "";
        if (attackingOwner.TryGetComponent<NetworkPlayerInfor>(out NetworkPlayerInfor infor))
        {
            nameKiller = infor.PlayerName;
        }
        else
        {
            nameKiller = "a boom";
        }

        return nameKiller;
    }

    private void CreateKillBox(string name)
    {
        string namePatient = GetComponent<NetworkPlayerInfor>().PlayerName;
        UIManager.Instance.CreateDisplayKillBox(name, namePatient);
    }
    private void TriggerDamageIndicator(Transform attacker)
    {
        if (attacker == null) { return; }
        DISystem.Instance.CreateIndicator(attacker);
    }
    [Command(requiresAuthority = false)]
    private void CmdTakeDamage(int amount, string name)
    {
        ServerTakeDamage(amount, name);

    }
    [Server]
    private void ServerTakeDamage(int amount, string nameKiller)
    {
        if (isDie) { return; }
        currentHealth = Mathf.Max(currentHealth - amount, 0);
        if (currentHealth == 0)
        {
            isDie = true;
            RpcDisplayKillBox(nameKiller);
        }

        SelfRescue();
    }
    [Server]
    private void SelfRescue()
    {
        if (healCoroutine != null) { StopCoroutine(healCoroutine); }
        healCoroutine = StartCoroutine(IncreaseHealth());
    }
    private IEnumerator IncreaseHealth()
    {
        yield return new WaitForSeconds(timeToHeal);
        while (currentHealth < maxHealth)
        {
            yield return null;
            currentHealth = Mathf.Min(currentHealth + increaseHealthAmount, maxHealth);
        }

    }
    [ClientRpc]
    private void RpcDisplayKillBox(string nameKiller)
    {
        CreateKillBox(nameKiller);
    }
    private void OnHandleDie(bool oldState, bool newState)
    {
        if (newState)
        {
            OnDie?.Invoke();
            if (isServer)
            {
                OnIncreasingScore?.Invoke(GetComponent<Team>().TeamName, 1);
            }
            
        }
    }
    private IEnumerator CountDownImmune()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(timeToImmune);
        canTakeDamage = true;
    }
    private void OnChangeCurrentHealth(int oldValue, int newValue)
    {
        if (!isOwned) { return; }
        if (newValue > lowHealth && oldValue <= lowHealth && oldValue != 0)
        {
            OnNormal?.Invoke();
        }
        if (oldValue < newValue)
        {
            OnRescuing?.Invoke();
        }
        else
        {
            if (currentHealth < lowHealth && currentHealth > 0)
            {
                OnNearlyDie?.Invoke();
            }
            OnTakeDamage?.Invoke();
        }
        OnChangeHealthBar?.Invoke((float)currentHealth / (float)maxHealth);

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
