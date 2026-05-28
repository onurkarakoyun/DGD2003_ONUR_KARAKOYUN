using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;              

public class MainMenuController : MonoBehaviour
{
    [Header("Paneller")]
    public GameObject mainMenuPanel; 
    public GameObject settingsPanel; 
    public GameObject saveSlotsPanel; // Yeni panelimiz!

    [Header("Ayarlar")]
    public Slider sensitivitySlider;

    void Start()
    {
        float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 2.0f);
        if (sensitivitySlider != null) sensitivitySlider.value = savedSensitivity;

        // Başlangıçta sadece Ana Menü açık olsun
        CloseAllPanels();
        mainMenuPanel.SetActive(true);
    }

    // --- PANEL YÖNETİMİ ---
    private void CloseAllPanels()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        saveSlotsPanel.SetActive(false);
    }
    

    public void OpenSettings()
    {
        CloseAllPanels();
        settingsPanel.SetActive(true);
    }

    public void OpenSaveSlots()
    {
        CloseAllPanels();
        saveSlotsPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        CloseAllPanels();
        mainMenuPanel.SetActive(true);
    }

    // --- OYUN BAŞLATMA (SAVE SLOT MANTIĞI) ---

    // Kayıtlı bir oyuna girmek için
    public void LoadGameSlot(int slotIndex)
    {
        // Hangi slotun seçildiğini PlayerPrefs'e kaydet (1, 2 veya 3)
        PlayerPrefs.SetInt("CurrentSaveSlot", slotIndex);
        
        // Yeni oyun olmadığını (mevcut kaydı yükleyeceğini) belirt
        PlayerPrefs.SetInt("IsNewGame", 0); 
        
        SceneManager.LoadScene("GameScene"); 
    }

    // '+' Butonuna basıldığında (Yeni Oyun)
    public void StartNewGame()
    {
        // Yeni bir oyun olduğunu belirt
        PlayerPrefs.SetInt("IsNewGame", 1); 
        
        // Yeni oyun için varsayılan bir slot seç (Örn: Slot 1'in üzerine yazsın veya yeni bir numara bulsun. Şimdilik 1 yapalım)
        PlayerPrefs.SetInt("CurrentSaveSlot", 1); 
        
        SceneManager.LoadScene("GameScene");
    }

    // --- DİĞER FONKSİYONLAR ---
    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetSensitivity(float value)
    {
        PlayerPrefs.SetFloat("MouseSensitivity", value);
        PlayerPrefs.Save(); 
    }
}