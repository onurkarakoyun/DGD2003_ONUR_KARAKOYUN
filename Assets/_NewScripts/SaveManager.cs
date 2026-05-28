using UnityEngine;
using System.IO; 
using System.Collections.Generic; // YENİ: Liste (List) kullanabilmek için gerekli!

[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;
    public int collectedFuses;
    public int collectedKeys;
    
    // YENİ: Alınan objelerin kimliklerini tutan liste
    public List<string> collectedItemIDs = new List<string>(); 
}

public class SaveManager : MonoBehaviour
{
    [Header("Referanslar")]
    public Transform playerTransform;
    public ElectricalPanel electricalPanel;

    private string saveFilePath;
    private int currentSlot;

    // YENİ: Oyun sırasında toplanan objelerin anlık listesi
    public List<string> currentCollectedItems = new List<string>();

    void Start()
    {
        currentSlot = PlayerPrefs.GetInt("CurrentSaveSlot", 1);
        saveFilePath = Application.persistentDataPath + "/saveData_" + currentSlot + ".json";

        int isNewGame = PlayerPrefs.GetInt("IsNewGame", 0);

        if (isNewGame == 1)
        {
            Debug.Log("YENİ OYUN BAŞLADI! Eski kayıt siliniyor...");
            if (File.Exists(saveFilePath)) File.Delete(saveFilePath);
        }
        else
        {
            if (File.Exists(saveFilePath)) LoadGame();
        }
    }

    // YENİ: Objeler alındıkça bu fonksiyon çalışıp onları listeye ekler
    public void RegisterCollectedItem(string objectID)
    {
        // Eğer bu ID listede yoksa ekle (Çift eklemeyi önler)
        if (!currentCollectedItems.Contains(objectID))
        {
            currentCollectedItems.Add(objectID);
        }
    }

    public void SaveGame()
    {
        GameData data = new GameData();
        data.playerPosition = playerTransform.position;
        data.collectedFuses = electricalPanel.currentFuses; 

        GameUIManager uiManager = GetComponent<GameUIManager>();
        if (uiManager != null) data.collectedKeys = uiManager.collectedKeys; 

        // Listeyi pakete koy
        data.collectedItemIDs = currentCollectedItems;

        string jsonText = JsonUtility.ToJson(data, true); 
        File.WriteAllText(saveFilePath, jsonText); 

        Debug.Log("Oyun Kaydedildi! Alınan obje sayısı: " + data.collectedItemIDs.Count);
    }

    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string jsonText = File.ReadAllText(saveFilePath);
            GameData data = JsonUtility.FromJson<GameData>(jsonText);

            CharacterController cc = playerTransform.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;
            playerTransform.position = data.playerPosition;
            if (cc != null) cc.enabled = true;

            electricalPanel.currentFuses = data.collectedFuses;

            GameUIManager uiManager = GetComponent<GameUIManager>();
            if (uiManager != null) uiManager.LoadDataToUI(data.collectedFuses, data.collectedKeys); 

            // Kayıtlı listeyi mevcut listemize eşitle
            currentCollectedItems = data.collectedItemIDs;

            // YENİ: SAHNEDEKİ ALINMIŞ OBJELERİ YOK ETME İŞLEMİ
            DestroyCollectedObjects();

            Debug.Log("Oyun Yüklendi! Silinen obje sayısı: " + currentCollectedItems.Count);
        }
    }

    private void DestroyCollectedObjects()
    {
        // Sahnedeki tüm Anahtarları bul
        KeyPickup[] allKeys = Object.FindObjectsOfType<KeyPickup>();
        foreach (KeyPickup key in allKeys)
        {
            // Eğer bu anahtarın ID'si alınmışlar listesinde varsa, onu sahneden sil!
            if (currentCollectedItems.Contains(key.objectID))
            {
                Destroy(key.gameObject);
            }
        }

        // Sahnedeki tüm Sigortaları bul
        FusePickup[] allFuses = FindObjectsOfType<FusePickup>();
        foreach (FusePickup fuse in allFuses)
        {
            // Eğer bu sigortanın ID'si alınmışlar listesinde varsa, onu sahneden sil!
            if (currentCollectedItems.Contains(fuse.objectID))
            {
                Destroy(fuse.gameObject);
            }
        }
    }
}