using UnityEngine;
using TMPro; 

public class GameUIManager : MonoBehaviour
{
    [Header("UI Metinleri")]
    public TextMeshProUGUI fuseText;
    public TextMeshProUGUI keyText;

    // SaveManager'ın bu sayıları okuyabilmesi için public yaptık!
    public int collectedFuses = 0;
    public int collectedKeys = 0;

    void OnEnable()
    {
        FusePickup.OnFuseCollected.AddListener(AddFuseUI);
        KeyPickup.OnKeyCollected.AddListener(AddKeyUI);
    }

    void OnDisable()
    {
        FusePickup.OnFuseCollected.RemoveListener(AddFuseUI);
        KeyPickup.OnKeyCollected.RemoveListener(AddKeyUI);
    }

    void Start()
    {
        UpdateUITexts();
    }

    private void AddFuseUI()
    {
        collectedFuses++;
        UpdateUITexts();
    }

    private void AddKeyUI(int keyID)
    {
        collectedKeys++;
        UpdateUITexts();
    }

    // JSON Yüklendiğinde değerleri eşitleyen fonksiyon
    public void LoadDataToUI(int loadedFuses, int loadedKeys)
    {
        collectedFuses = loadedFuses;
        collectedKeys = loadedKeys;
        UpdateUITexts();
    }

    private void UpdateUITexts()
    {
        if (fuseText != null) fuseText.text = "Fuse: " + collectedFuses + "/3";
        if (keyText != null) keyText.text = "Key: " + collectedKeys;
    }
}