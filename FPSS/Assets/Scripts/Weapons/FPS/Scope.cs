using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : MonoBehaviour
{
    private const int FOVDefault = 60;

    [Range(0, 1f)][SerializeField] private float sensitivity = 0.1f;
    [SerializeField] PlayerController playerController;
    [SerializeField] ReferenceManager referenceManager;
    [SerializeField] private GameObject[] lens;
    [SerializeField] private GameObject crossHair;


    public void ScopeUp()
    {
        foreach (var obj in lens)
        {
            obj.SetActive(false);
        }
        crossHair.SetActive(true);
        referenceManager.FPSController.SetRotatateSensitivity(sensitivity);
    }
    public void ScopeDown()
    {
        foreach (var obj in lens)
        {
            obj.SetActive(true);
        }
        crossHair.SetActive(false);
        referenceManager.FPSController.SetRotatateSensitivity(2f);
    }
}
