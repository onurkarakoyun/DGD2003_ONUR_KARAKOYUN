using UnityEngine;
using UnityEngine.Events;

public class KeyPickup : MonoBehaviour
{
    [Header("Anahtar Ayarları")]
    public int keyID = 1;

    public static UnityEvent<int> OnKeyCollected = new UnityEvent<int>();

    public void Collect()
    {
        Debug.Log("Anahtar (ID: " + keyID + ") alındı!");
        
        OnKeyCollected.Invoke(keyID); 
        
        Destroy(gameObject); 
    }
}