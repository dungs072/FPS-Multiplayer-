using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISelection : MonoBehaviour
{
    public static UISelection Instance { get; private set; }
    [SerializeField] private GameObject backButton;
    
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
    }
}
