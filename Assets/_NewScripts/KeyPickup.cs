using UnityEngine;
using UnityEngine.Events;

public class KeyPickup : MonoBehaviour
{
    [Header("Anahtar Ayarları")]
    public int keyID = 1; 
    
    // YENİ: Bu objeye özel benzersiz isim (Örn: "Anahtar_1", "Kutuphane_Anahtari")
    public string objectID; 

    public static UnityEvent<int> OnKeyCollected = new UnityEvent<int>();

    public void Collect()
    {
        Debug.Log("Anahtar (ID: " + keyID + ") alındı!");
        
        // YENİ: SaveManager'ı bul ve bu objenin alındığını listeye ekle
        SaveManager saveManager = Object.FindFirstObjectByType<SaveManager>();
        if (saveManager != null)
        {
            saveManager.RegisterCollectedItem(objectID);
        }

        OnKeyCollected.Invoke(keyID); 
        Destroy(gameObject); 
    }
}