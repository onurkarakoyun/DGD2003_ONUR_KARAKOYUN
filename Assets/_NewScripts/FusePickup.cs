using UnityEngine;
using UnityEngine.Events;

public class FusePickup : MonoBehaviour
{
    // YENİ: Bu objeye özel benzersiz isim (Örn: "Sigorta_1", "Sigorta_2")
    public string objectID;

    public static UnityEvent OnFuseCollected = new UnityEvent();

    public void Collect()
    {
        Debug.Log("Bir sigorta (Fuse) buldun!");
        
        // YENİ: SaveManager'ı bul ve bu objenin alındığını listeye ekle
        SaveManager saveManager = FindObjectOfType<SaveManager>();
        if (saveManager != null)
        {
            saveManager.RegisterCollectedItem(objectID);
        }

        OnFuseCollected.Invoke();
        Destroy(gameObject);
    }
}