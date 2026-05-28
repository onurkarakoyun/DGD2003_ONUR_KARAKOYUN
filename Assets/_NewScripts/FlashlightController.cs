using UnityEngine;
using UnityEngine.UI; // Slider (UI) kullanmak için bu kütüphane şart!

public class FlashlightController : MonoBehaviour
{
    [Header("Ayarlar")]
    public Light flashlight; 
    public KeyCode toggleKey = KeyCode.F; 

    [Header("Batarya Sistemi")]
    public float maxBattery = 100f;      
    public float currentBattery;         
    public float drainRate = 5f;     // Açıken saniyede ne kadar azalacak?
    public float rechargeRate = 3f;  // Kapalıyken saniyede ne kadar dolacak?

    [Header("Arayüz (UI)")]
    public Slider batterySlider;     // Ekranda bataryayı gösterecek çubuk

    private bool isFlashlightOn = false;

    void Start()
    {
        currentBattery = maxBattery;
        
        // Slider'ın maksimum değerini bataryamızın maksimumuna eşitle
        if (batterySlider != null)
        {
            batterySlider.maxValue = maxBattery;
            batterySlider.value = currentBattery;
        }
        
        if (flashlight != null) flashlight.enabled = isFlashlightOn;
    }

    void Update()
    {
        // Feneri açma / kapama
        if (Input.GetKeyDown(toggleKey))
        {
            // Şarj sıfırsa açmaya çalışma
            if (!isFlashlightOn && currentBattery <= 0) return;

            isFlashlightOn = !isFlashlightOn;
            if (flashlight != null) flashlight.enabled = isFlashlightOn;
        }

        // ŞARJ AZALMA VE DOLMA MANTIĞI
        if (isFlashlightOn)
        {
            // Fener açıksa şarjı azalt
            currentBattery -= drainRate * Time.deltaTime;

            if (currentBattery <= 0)
            {
                currentBattery = 0;
                isFlashlightOn = false; // Otomatik kapat
                if (flashlight != null) flashlight.enabled = false;
            }
        }
        else
        {
            // Fener kapalıysa şarjı doldur
            if (currentBattery < maxBattery)
            {
                currentBattery += rechargeRate * Time.deltaTime;
                if (currentBattery > maxBattery) currentBattery = maxBattery;
            }
        }

        // Arayüzü (Slider'ı) güncelle
        if (batterySlider != null)
        {
            batterySlider.value = currentBattery;
        }
    }
}