using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustmentSelection : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform frontViewPosition;
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickSound;
    private TargetSelection targetSelection;

    private void Update() {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                if(hit.collider.TryGetComponent<TargetSelection>(out TargetSelection target))
                {
                    target.PopUpToMainCamera(frontViewPosition.position);
                    targetSelection = target;
                    UISelection.Instance.ToggleBackButton(true);
                    PlayClickSound();
                }
            }
        }
    }
    public void PutTargetBack()
    {
        if(targetSelection==null){return;}
        targetSelection.PutTargetBack();
        UISelection.Instance.ToggleBackButton(false);
    }
    private void PlayClickSound()
    {
        if(TargetSelection.IsPopUpMainCamera){return;}
        audioSource.PlayOneShot(clickSound);
    }
}
