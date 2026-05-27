using UnityEngine;
using System.IO; // Dosya okuma/yazma işlemleri için şart

// Kaydedilecek verilerin şablonunu oluşturuyoruz.
// System.Serializable olmazsa JSON'a çevrilememez!
[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;
    public int collectedFuses;
}

public class SaveManager : MonoBehaviour
{
    [Header("Referanslar")]
    public Transform playerTransform;
    public ElectricalPanel electricalPanel;

    private string saveFilePath;

    void Start()
    {
        // Kayıt dosyasının bilgisayardaki gizli/güvenli yerini belirliyoruz (AppData klasörü)
        saveFilePath = Application.persistentDataPath + "/saveData.json";
    }

    void Update()
    {
        // Hızlı test için: F5 Kaydet, F9 Yükle
        if (Input.GetKeyDown(KeyCode.F5)) SaveGame();
        if (Input.GetKeyDown(KeyCode.F9)) LoadGame();
    }

    public void SaveGame()
    {
        // Şablondan yeni bir veri paketi oluştur ve içini doldur
        GameData data = new GameData();
        data.playerPosition = playerTransform.position;
        data.collectedFuses = electricalPanel.currentFuses; // Kaç sigorta toplandığını al

        // Veriyi JSON formatında bir metne çevir (true parametresi kodun okunabilir alt alta yazılmasını sağlar)
        string jsonText = JsonUtility.ToJson(data, true); 
        
        // Metni dosyaya yaz
        File.WriteAllText(saveFilePath, jsonText); 

        Debug.Log("Oyun JSON ile Kaydedildi! Dosya Yolu: " + saveFilePath);
    }

    public void LoadGame()
    {
        // Dosya gerçekten var mı diye kontrol et
        if (File.Exists(saveFilePath))
        {
            // Dosyadaki metni oku
            string jsonText = File.ReadAllText(saveFilePath);
            
            // Metni tekrar bizim GameData sınıfımıza çevir
            GameData data = JsonUtility.FromJson<GameData>(jsonText);

            // 1. OYUNCU POZİSYONUNU YÜKLE
            // CharacterController varken pozisyonu direkt değiştirmek bug yapabilir, bu yüzden anlık kapatıp açıyoruz.
            CharacterController cc = playerTransform.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;
            playerTransform.position = data.playerPosition;
            if (cc != null) cc.enabled = true;

            // 2. SİGORTA SAYISINI YÜKLE
            electricalPanel.currentFuses = data.collectedFuses;

            Debug.Log("Oyun JSON'dan Yüklendi! Sigorta: " + data.collectedFuses);
        }
        else
        {
            Debug.LogWarning("Kayıt dosyası bulunamadı!");
        }
    }
}
