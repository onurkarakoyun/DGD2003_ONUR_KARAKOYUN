using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("Paneller")]
    public GameObject gameOverPanel;

    // Düşman bizi yakaladığında bu fonksiyon çalışacak
    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true); // Paneli aç
        Time.timeScale = 0f; // Zamanı durdur

        // Fareyi serbest bırak ve görünür yap
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Yeniden Başla butonuna basıldığında
    public void RestartGame()
    {
        Time.timeScale = 1f; // Zamanı normale döndür
        
        // Mevcut sahneyi baştan yükle (Bu sayede SaveManager otomatik olarak son kaydı yükler!)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Ana Menü butonuna basıldığında
    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Zamanı normale döndür
        SceneManager.LoadScene("MainMenu");
    }
}