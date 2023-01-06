using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelection : MonoBehaviour
{
    [SerializeField] private Transform upPosition;
    [SerializeField] private Transform standPosition;
    [SerializeField] private Transform gun;
    [SerializeField] private AudioSource audioSource;
    private Vector3 putBackPosition;
    // private Renderer[] renders;
    private bool isMouseIn;
    private static bool isPopUpMainCamera;
    private Vector3 targetPosition;
    public static bool IsPopUpMainCamera{get{ return isPopUpMainCamera;}}
    private void Start() {
        putBackPosition = transform.position;
    }
    private void Update() {
        if(isPopUpMainCamera){return;}
        if(!isMouseIn&&gun.position==standPosition.position){return;}
        if(isMouseIn)
        {
            targetPosition = upPosition.position;
        }
        else
        {
            targetPosition = standPosition.position;
        }
        gun.position = Vector3.Lerp(gun.position,targetPosition,Time.deltaTime*5f);
        
    }
    private void OnMouseEnter() {
        if(isPopUpMainCamera){return;}
        isMouseIn = true;
        audioSource.Play();
        
        
    }
    private void OnMouseExit() {
        isMouseIn = false;
    }
   
    public void PopUpToMainCamera(Vector3 pos)
    {
        if(isPopUpMainCamera){return;}
        isPopUpMainCamera = true;
        transform.position = pos;
        gun.localEulerAngles = new Vector3(90,0,0);
        GetComponent<Collider>().enabled = false;
        gun.GetComponent<Collider>().enabled = true;
    }
    public void PutTargetBack()
    {
        gun.localEulerAngles = Vector3.zero;
        transform.position = putBackPosition;
        isPopUpMainCamera = false;
        gun.GetComponent<Collider>().enabled = false;
        GetComponent<Collider>().enabled = true;
        

    }

}
