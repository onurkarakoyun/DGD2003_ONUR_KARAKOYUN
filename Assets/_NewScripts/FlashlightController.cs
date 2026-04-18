using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    [Header("Ayarlar")]
    public Light flashlight;
    public KeyCode toggleKey = KeyCode.F;

    private bool isFlashlightOn = false;

    void Start()
    {
        if (flashlight != null)
        {
            flashlight.enabled = isFlashlightOn;
        }
    }

    void Update()
    {
        // F tuşuna basıldığında
        if (Input.GetKeyDown(toggleKey))
        {
            isFlashlightOn = !isFlashlightOn;
            if (flashlight != null)
            {
                flashlight.enabled = isFlashlightOn;
            }
        }
    }
}
