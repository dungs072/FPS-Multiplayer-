using UnityEngine;
using Mirror;

[RequireComponent(typeof(PlayerController))]
public class EffectNetworkManager : NetworkBehaviour
{
    [SerializeField] private MeleeEffectAttribute meleeEffectAttribute;
    [SerializeField] private MeleeSoundAttribute meleeSoundAttribute;
    [SerializeField] private AudioSource audioSource;

    [Command]
    public void CmdSpawnWoodEffectWithoutDecal(Vector3 pos, Quaternion quaternion, EffectNetworkType effectType)
    {
        RpcSpawnWoodEffectWithoutDecal(pos, quaternion, effectType);
    }
    [ClientRpc]
    private void RpcSpawnWoodEffectWithoutDecal(Vector3 pos, Quaternion quaternion, EffectNetworkType effectType)
    {
        if (isOwned) { return; }
        if (effectType == EffectNetworkType.WoodWithoutDecal)
        {
            var hitEffectInstance = Instantiate(meleeEffectAttribute.WoodEffectWithoutDecal,
                               pos, quaternion);
            audioSource.PlayOneShot(meleeSoundAttribute.ObstacleHitAudio);
            Destroy(hitEffectInstance, 1f);
        }
        else if(effectType==EffectNetworkType.BloodWithoutDecal)
        {
            var hitEffectInstance = Instantiate(meleeEffectAttribute.BloodEffectWithoutDecal,
                               pos, quaternion);
            audioSource.PlayOneShot(meleeSoundAttribute.BodyHitAudio);
            Destroy(hitEffectInstance, 1f);
        }

    }

}
public enum EffectNetworkType
{
    WoodWithoutDecal,
    BloodWithoutDecal
}
