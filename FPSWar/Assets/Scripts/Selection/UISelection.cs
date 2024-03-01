using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISelection : MonoBehaviour
{
    public static UISelection Instance { get; private set; }
    [SerializeField] private GameObject backButton;
    
    [field:SerializeField] public Transform ScopePack{get;private set;}
    
    private void Awake()//have to change dontDestroyOnLoad
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    public void ToggleBackButton(bool state)
    {
        backButton.SetActive(state);
        ToggleScopePackUI(state);
    }
    private void ToggleScopePackUI(bool state)
    {
        if(state)
        {
            ClearScopePackUI();
        }
        ScopePack.gameObject.SetActive(state);
    }
    private void ClearScopePackUI()
    {
         foreach(Transform child in ScopePack)
        {
            Destroy(child.gameObject);
        }
    }
}
