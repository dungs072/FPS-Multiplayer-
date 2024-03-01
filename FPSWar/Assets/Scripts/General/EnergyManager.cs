using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    [SerializeField] private int MaxEnergy;
    [SerializeField] private float timePerIncrease = 0.5f;
    [SerializeField] private float timePerDecrease = 0.5f;
    [SerializeField] private float timeStartIncreaseAfterDecrease = 3f;
    [SerializeField] private int changeValueEnergy = 1;
    [Header("Sound")]
    [SerializeField] private AudioClip outOfBreathSound;
    private int currentEnergy;
    private float currentDecreaseTime = 0f;
    private Coroutine increaseCoroutine;
    public int CurrentEnergy{get{return currentEnergy;}}
    private void Start()
    {
        currentEnergy = MaxEnergy;
        currentDecreaseTime = 0f;
    }

    public void DecreaseEnergy(float deltaTime)
    {
        if (currentEnergy == 0f) { return; }
        if(increaseCoroutine!=null)
        {
            StopCoroutine(increaseCoroutine);
            increaseCoroutine = null;
        }
        if (currentDecreaseTime >= timePerDecrease)
        {
            currentEnergy = Math.Max(currentEnergy - changeValueEnergy, 0);
            UpdateEnergyBarUI();
            currentDecreaseTime = 0f;
            if(currentEnergy==0)
            {
                GetComponent<AudioSource>().PlayOneShot(outOfBreathSound);
            }
        }
        else
        {
            currentDecreaseTime += deltaTime;
        }

    }
    public void StartIncreaseEnergy()
    {
        if(currentEnergy==MaxEnergy){return;}
        increaseCoroutine = StartCoroutine(IncreaseEnergy());
    }
    private IEnumerator IncreaseEnergy()
    {
        yield return new WaitForSeconds(timeStartIncreaseAfterDecrease);
        while (currentEnergy != MaxEnergy)
        {
            currentEnergy = Math.Min(currentEnergy + changeValueEnergy, MaxEnergy);
            UpdateEnergyBarUI();
            yield return new WaitForSeconds(timePerIncrease);
        }
    }
    private void UpdateEnergyBarUI()
    {
        float energyUIAmount = (float)currentEnergy / (float)MaxEnergy;
        UIManager.Instance.ChangeEnergyBar(energyUIAmount);
    }
}
