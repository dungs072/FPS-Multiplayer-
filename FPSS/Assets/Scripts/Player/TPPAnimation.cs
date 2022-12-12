using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPPAnimation : MonoBehaviour
{
    [SerializeField] private RigManager rigManager;
    [SerializeField] private ReferenceManager refer;
    [Header("Rocket")]
    [SerializeField] private GameObject rocketAtHand;
    #region Animation
    public void TurnOnHandsWeight(int state)
    {
        rigManager.SetHandWeight(1f);
        if (state==1) { return; }
        rigManager.SetSecondHandGrabWeight(1f);
    }
    public void ToggleBothHands(float value)
    {
        rigManager.SetSecondHandGrabWeight(value);
        rigManager.SetHandWeight(value);
    }
    public void ToggleSecondHandGrab(float value)
    {
        rigManager.SetSecondHandGrabWeight(value);
    }
    public void ToggleRig(float value)
    {
        rigManager.SetRigWeight(value);
    }
    
    public void ToggleOffReloadRocketAnimation()
    {
        GameObject rocketAtGun = refer.TPPController.CurrentWeapon.GetRocketAtGun();
        if(rocketAtGun==null){return;}
        rocketAtGun.SetActive(true);
        rocketAtHand.SetActive(false);
    }
    public void ToggleOnReloadRocketAnimation()
    {
        GameObject rocketAtGun = refer.TPPController.CurrentWeapon.GetRocketAtGun();
        if(rocketAtGun==null){return;}
        rocketAtGun.SetActive(false);
        rocketAtHand.SetActive(true);
    }
    public void ToggleOnFireRocket()
    {
        GameObject rocketAtGun = refer.TPPController.CurrentWeapon.GetRocketAtGun();
        if(rocketAtGun==null){return;}
        rocketAtGun.SetActive(false);
    }
    public void ToggleOffFireRocket()
    {
        GameObject rocketAtGun = refer.TPPController.CurrentWeapon.GetRocketAtGun();
        if(rocketAtGun==null){return;}
        rocketAtGun.SetActive(true);
    }
    #endregion
}
