using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkPlayerManager : NetworkBehaviour
{
    [SerializeField] GameObject[] nonLocalObjects;
    [SerializeField] Camera[] cameras;
    [SerializeField] AudioListener audioListener;
    [SerializeField] GameObject[] TPPLayerObjects;
    [SerializeField] GameObject[] TPPSLayerObjects;
    [SerializeField] GameObject[] FPSSLayerObjects;
    [SerializeField] GameObject[] ragdollLayerObjects;
    [SerializeField] SkinnedMeshRenderer tppMesh;
    [Header("Map")]
    [SerializeField] GameObject mapCamera;
    private const string tppLayer = "TPP";
    private const string tppsLayer = "TPPs";
    private const string fpssLayer = "FPSs";
    private const string ragdollLayer = "Ragdoll";
    private void Start()
    {
        if(isOwned)
        {
            HandleOwnedPlayer();
        }
        else
        {
            HandleOtherPlayers();
        }
    }
    private void HandleOwnedPlayer()
    {
        foreach(var obj in TPPLayerObjects)
        {
            SetGameLayerRecursive(obj, tppLayer);
        }
        foreach(var obj in ragdollLayerObjects)
        {
            obj.layer = LayerMask.NameToLayer(ragdollLayer);
        }
        //ragdollManager.ToggleColliders(false);
    }
    private void HandleOtherPlayers()
    {
        mapCamera.SetActive(false);
        ChangeShadowOnlyMesh(false);
        foreach (var obj in nonLocalObjects)
        {
            obj.gameObject.SetActive(false);
        }
        foreach (var camera in cameras)
        {
            camera.enabled = false;
        }
        audioListener.enabled = false;

        foreach(var obj in TPPSLayerObjects)
        {
            SetGameLayerRecursive(obj, tppsLayer);
        }
        foreach(var obj in FPSSLayerObjects)
        {
            SetGameLayerRecursive(obj, fpssLayer);
        }
    }
    public void ChangeShadowOnlyMesh(bool state)
    {
        ToggleMeshRenderer(!state);
    }
    public void ChangeLayerFPSWeapon(GameObject weapon)
    {
        if(isOwned){return;}
        SetGameLayerRecursive(weapon,fpssLayer);
    }
    private void SetGameLayerRecursive(GameObject _go, string nameLayer)
    {
        _go.layer = LayerMask.NameToLayer(nameLayer);
        foreach (Transform child in _go.transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer(nameLayer);

            Transform _HasChildren = child.GetComponentInChildren<Transform>();
            if (_HasChildren != null)
                SetGameLayerRecursive(child.gameObject, nameLayer);

        }
    }
    public void ChangeObjectToTppsLayer(GameObject gObject)
    {
        SetGameLayerRecursive(gObject,tppsLayer);
    }
    public void ToggleMeshRenderer(bool state)
    {
        if(state)
        {
            tppMesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
        else
        {
            tppMesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }
        
    }
    public void ChangeMeshRendererShadow(GameObject meshObject,bool isShadowOnly)
    {
        if(meshObject.TryGetComponent<MeshRenderer>(out MeshRenderer m))
        {
            if(!isShadowOnly)
            {
                m.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            }
            else
            {
                m.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            }
        }
        foreach(Transform child in meshObject.transform)
        {
            if(child.TryGetComponent<MeshRenderer>(out MeshRenderer mesh))
            {
                if(!isShadowOnly)
                {
                    mesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                }
                else
                {
                    mesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                }
            }
           
            Transform _HasChildren = child.GetComponentInChildren<Transform>();
            if(_HasChildren!=null)
            {
                ChangeMeshRendererShadow(child.gameObject,isShadowOnly);
            }
        }
        
    }
}