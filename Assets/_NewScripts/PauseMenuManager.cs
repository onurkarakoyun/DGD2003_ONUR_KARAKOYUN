using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Slider için gerekli

public class PauseMenuManager : MonoBehaviour
{
    [Header("Paneller")]
    public GameObject pausePanel;
    public GameObject settingsPanel; 

    [Header("Ayarlar Sistemi")]
    public Slider sensitivitySlider;
    public FPSController playerController; // Anında fare hızını değiştirmek için oyuncu referansı

    private bool isPaused = false;

    void Start()
    {
        // Oyun başında kaydedilmiş hassasiyeti bul ve Slider'a eşitle
        if (sensitivitySlider != null)
        {
            sensitivitySlider.value = PlayerPrefs.GetFloat("MouseSensitivity", 2.0f);
        }
    }

    void Update()
    {
        // ESC tuşuna basıldığında
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        settingsPanel.SetActive(false); // Garanti olsun diye ayarları kapalı tutuyoruz
        Time.timeScale = 0f; 
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false); 
        
        Time.timeScale = 1f; 
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // --- AYARLAR (SETTINGS) BUTON FONKSİYONLARI ---

    public void OpenSettings()
    {
        pausePanel.SetActive(false); // Pause menüsünü gizle
        settingsPanel.SetActive(true); // Ayarları aç
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false); // Ayarları gizle
        pausePanel.SetActive(true); // Pause menüsünü geri aç
    }

    // Slider kaydırıldıkça çalışacak fonksiyon
    public void SetSensitivity(float value)
    {
        // 1. Bilgisayara kaydet
        PlayerPrefs.SetFloat("MouseSensitivity", value);
        PlayerPrefs.Save(); 
        
        // 2. Oyuncunun fare hızını anında değiştir!
        if (playerController != null)
        {
            playerController.mouseSensitivity = value;
        }
    }

    // --- DİĞER BUTON FONKSİYONLARI ---

    public void SaveGameBtn()
    {
        SaveManager saveManager = Object.FindAnyObjectByType<SaveManager>();
        if (saveManager != null)
        {
            saveManager.SaveGame();
            Debug.Log("Oyun kaydedildi!");
        }
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenu");
    }
}