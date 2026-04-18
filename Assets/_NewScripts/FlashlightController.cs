using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    [Header("Ayarlar")]
    public Light flashlight; 
    public KeyCode toggleKey = KeyCode.F; 

    [Header("Batarya Sistemi")]
    public float maxBattery = 100f;
    public float currentBattery;
    public float drainRate = 5f;

    private bool isFlashlightOn = false;

    void Start()
    {
        currentBattery = maxBattery;
        
        if (flashlight != null)
        {
            flashlight.enabled = isFlashlightOn;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(toggleKey) && currentBattery > 0)
        {
            isFlashlightOn = !isFlashlightOn;
            
            if (flashlight != null)
            {
                flashlight.enabled = isFlashlightOn;
            }
        }
        if (isFlashlightOn)
        {
            currentBattery -= drainRate * Time.deltaTime;

            if (currentBattery <= 0)
            {
                currentBattery = 0;
                isFlashlightOn = false;
                
                if (flashlight != null)
                {
                    flashlight.enabled = false;
                }
                Debug.Log("Fenerin şarjı bitti! Elektrikleri açmalısın.");
            }
        }
    }
}