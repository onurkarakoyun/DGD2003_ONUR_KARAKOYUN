using UnityEngine;
using UnityEngine.Events;

public class ElectricalPanel : MonoBehaviour
{
    [Header("Pano Ayarları")]
    public int requiredFuses = 3;
    private int currentFuses = 0;
    private bool isPowerOn = false;

    [Header("Okul Işıkları")]
    public Light[] schoolLights; 

    public static UnityEvent OnPowerRestored = new UnityEvent();

    void OnEnable()
    {
        FusePickup.OnFuseCollected.AddListener(AddFuse);
    }

    void OnDisable()
    {
        FusePickup.OnFuseCollected.RemoveListener(AddFuse);
    }
    private void AddFuse()
    {
        currentFuses++;
        Debug.Log("Panoya bir sigorta eklendi! Durum: " + currentFuses + "/" + requiredFuses);
    }

    public void Interact()
    {
        if (isPowerOn)
        {
            Debug.Log("Elektrik zaten açık!");
            return;
        }
        if (currentFuses >= requiredFuses)
        {
            Debug.Log("Tüm sigortalar tamam! Şalteri indirdin ve elektrikler açıldı!");
            TurnOnLights();
            isPowerOn = true;
            
            OnPowerRestored.Invoke(); 
        }
        else
        {
            Debug.Log("Sistem çalışmıyor! Eksik sigortalar var. Gereken: " + requiredFuses + ", Toplanan: " + currentFuses);
        }
    }

    private void TurnOnLights()
    {
        foreach (Light light in schoolLights)
        {
            if (light != null)
            {
                light.enabled = true; 
            }
        }
    }
}