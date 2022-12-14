using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : MonoBehaviour
{
    private const int FOVDefault = 60;
    [SerializeField] private Camera fpsCamera;
    [SerializeField] private GameObject gunModel;
    [SerializeField] private GameObject muzzleFlash;
    [Range(0, 1f)][SerializeField] private float sensitivity = 0.1f;
    [SerializeField] private float timeAppearScopeOverLay = 1f;
    [SerializeField] private int FOVAimScope = 2;
    [SerializeField] PlayerController playerController;
    [SerializeField] ReferenceManager referenceManager;
    [SerializeField] private GameObject[] lens;
    private Coroutine scopeUpCoroutine;
    private void Update()
    {
        if (!playerController.isOwned) { return; }
        if (Input.GetMouseButtonDown(1))
        {
            ScopeUp();
        }
        if (Input.GetMouseButtonUp(1))
        {
            if (scopeUpCoroutine != null)
            {
                StopCoroutine(scopeUpCoroutine);
            }
            ScopeDown();
        }

    }
    private void ScopeUp()
    {
        if(scopeUpCoroutine!=null){StopCoroutine(scopeUpCoroutine);}
        scopeUpCoroutine = StartCoroutine(HandleScopeUp());
    }
    private IEnumerator HandleScopeUp()
    {
        muzzleFlash.SetActive(false);
        yield return new WaitForSeconds(timeAppearScopeOverLay);
        foreach (var obj in lens)
        {
            obj.SetActive(false);
        }
        referenceManager.FPSController.SetRotatateSensitivity(sensitivity);
        gunModel.SetActive(false);
        UIManager.Instance.ToggleCrossHairScope(true);
        fpsCamera.fieldOfView = FOVAimScope;

    }
    private void ScopeDown()
    {
        foreach (var obj in lens)
        {
            obj.SetActive(true);
        }
        muzzleFlash.SetActive(true);
        UIManager.Instance.ToggleCrossHairScope(false);
        referenceManager.FPSController.SetRotatateSensitivity(2f);
        fpsCamera.fieldOfView = FOVDefault;
        gunModel.SetActive(true);
    }
}
