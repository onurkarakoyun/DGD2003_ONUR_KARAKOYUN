using UnityEngine;
using UnityEngine.AddressableAssets; // Addressables kod kütüphanesi
using UnityEngine.ResourceManagement.AsyncOperations;

public class KeycardSpawner : MonoBehaviour
{
    [Header("Addressables Ayarları")]
    // Objenin ismini yazmak yerine Inspector'dan direkt seçmemizi sağlayan güvenli değişken tipi
    public AssetReference keycardPrefab; 
    public Transform spawnPoint; // Kartın nerede belireceği

    void OnEnable()
    {
        // Elektrik panosu "OnPowerRestored" eventini fırlattığında bu kodu dinlemeye başla
        ElectricalPanel.OnPowerRestored.AddListener(SpawnKeycard);
    }

    void OnDisable()
    {
        ElectricalPanel.OnPowerRestored.RemoveListener(SpawnKeycard);
    }

    public void SpawnKeycard()
    {
        Debug.Log("Elektrik geldi! Addressables ile Keycard yükleniyor...");
        
        // Addressables ile asenkron yükleme ve oluşturma (Instantiate) işlemi.
        // Asenkron demek, oyun donmadan arka planda yüklenmesi demektir.
        keycardPrefab.InstantiateAsync(spawnPoint.position, spawnPoint.rotation).Completed += OnKeycardLoaded;
    }

    // Yükleme tamamlandığında çalışacak olan geri bildirim fonksiyonu
    private void OnKeycardLoaded(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Addressables Başarılı: Keycard sahneye yüklendi!");
        }
        else
        {
            Debug.LogError("Addressables Hatası: Keycard yüklenemedi!");
        }
    }
}
