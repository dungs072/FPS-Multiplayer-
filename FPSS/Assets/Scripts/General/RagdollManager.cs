using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class RagdollManager : NetworkBehaviour
{
    [SerializeField] private Animator TPPAnimator;
    [SerializeField] private List<Rigidbody> rbs;
    [SerializeField] private List<Collider> colliders;
    [SerializeField] private HealthManager healthManager;
    [SerializeField] private RespawnManager respawnManager;
    [SerializeField] private Collider playerCollider;
    [SerializeField] private Collider leftFootCollider;
    [SerializeField] private Collider rightFootCollider;

    private void Start()
    {
        if (isOwned)
        {
            ToggleRagdoll(true);

        }
        if (isServer)
        {
            healthManager.OnDie += ServerTurnOnRagdoll;
        }
        else
        {
            healthManager.OnDie += ClientTurnOnRagdoll;
        }
        healthManager.OnDie += OnDie;
        respawnManager.OnRespawn += OnRescue;

    }
    private void OnDestroy()
    {
        if (isServer)
        {
            healthManager.OnDie -= ServerTurnOnRagdoll;
        }
        else
        {
            healthManager.OnDie -= ClientTurnOnRagdoll;
        }
        healthManager.OnDie -= OnDie;
        respawnManager.OnRespawn -= OnRescue;

    }
    private void ClientTurnOnRagdoll()
    {
        TPPAnimator.enabled = false;
        playerCollider.enabled = false;
    }

    private void ServerTurnOnRagdoll()
    {
        ToggleRagdoll(false);
    }
    public void TurnOffRagdoll()
    {
        ToggleRagdoll(true);
    }
    public void ToggleRagdoll(bool state)
    {
        foreach (var rb in rbs)
        {
            rb.isKinematic = state;
        }
        TPPAnimator.enabled = state;
        playerCollider.enabled = state;
    }
    public void ToggleColliders(bool state)
    {
        foreach (var collider in colliders)
        {
            collider.enabled = state;
        }
    }
    public void ToggleFoot(bool isTurnOnColliderFoot)
    {
        leftFootCollider.enabled = isTurnOnColliderFoot;
        rightFootCollider.enabled = isTurnOnColliderFoot;
    }
    private void OnDie()
    {
        ToggleFoot(true);
    }
    private void OnRescue()
    {
        ToggleFoot(false);
    }

}
