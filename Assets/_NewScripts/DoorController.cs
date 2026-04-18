using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Kapı Ayarları")]
    public int doorID = 1;       
    public float openSpeed = 2f; 
    
    [Header("Efektler")]
    public ParticleSystem doorOpenVFX;

    private bool isUnlocked = false;
    private bool isOpened = false;
    private Quaternion targetRotation;

    void Start()
    {
        targetRotation = transform.localRotation; 
    }

    void OnEnable() { KeyPickup.OnKeyCollected.AddListener(CheckAndUnlock); }
    void OnDisable() { KeyPickup.OnKeyCollected.RemoveListener(CheckAndUnlock); }

    private void CheckAndUnlock(int collectedKeyID)
    {
        if (collectedKeyID == doorID)
        {
            isUnlocked = true;
            Debug.Log("Doğru anahtar bulundu! Kapı kilidi açıldı.");
        }
    }

    public void TryOpenDoor()
    {
        if (isUnlocked && !isOpened)
        {
            isOpened = true;
            targetRotation = Quaternion.Euler(
                transform.localEulerAngles.x, 
                transform.localEulerAngles.y + 90f, 
                transform.localEulerAngles.z
            );
            
            if (doorOpenVFX != null)
            {
                doorOpenVFX.Play();
            }

            CameraManager camManager = Object.FindFirstObjectByType<CameraManager>();
            if (camManager != null && doorID == 2)
            {
                camManager.ActivateExitCamera();
            }
        }
    }

    void Update()
    {
        if (isOpened)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * openSpeed);
        }
    }
}