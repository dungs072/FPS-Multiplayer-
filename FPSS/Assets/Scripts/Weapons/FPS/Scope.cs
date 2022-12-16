using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
public class Scope : MonoBehaviour
{
    private const int FOVDefault = 60;

    [Range(0, 1f)][SerializeField] private float sensitivity = 0.1f;
    [SerializeField] private GameObject[] lens;
    [SerializeField] private GameObject crossHair;
    [SerializeField] private GameObject displayScope;
    private FirstPersonController fps;

    public void SetFPSController(FirstPersonController fps)
    {
        this.fps = fps;
    }

    public void ScopeUp()
    {
        foreach (var obj in lens)
        {
            obj.SetActive(false);
        }
        crossHair.SetActive(true);
        displayScope.SetActive(true);
        fps.SetRotatateSensitivity(sensitivity);
    }
    public void ScopeDown()
    {
        foreach (var obj in lens)
        {
            obj.SetActive(true);
        }
        crossHair.SetActive(false);
        displayScope.SetActive(false);
        fps.SetRotatateSensitivity(2f);
    }
}
