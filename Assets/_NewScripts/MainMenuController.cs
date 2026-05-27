using UnityEngine;
using UnityEngine.SceneManagement; // Sahneler arası geçiş için gerekli
using UnityEngine.UI;              // Slider gibi UI elemanlarını kullanmak için gerekli

public class MainMenuController : MonoBehaviour
{
    [Header("Ayarlar")]
    public Slider sensitivitySlider;

    void Start()
    {
        // Oyun açıldığında daha önce kaydedilmiş hassasiyet değerini bul.
        // Eğer daha önce hiç kaydedilmemişse, varsayılan olarak 2.0f değerini ver.
        float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 2.0f);
        
        if (sensitivitySlider != null)
        {
            sensitivitySlider.value = savedSensitivity;
        }
    }

    // Play butonuna basıldığında çalışacak
    public void PlayGame()
    {
        // "GameScene" yazan yer, oyun sahnemizin tam adıyla aynı olmalı
        SceneManager.LoadScene("GameScene"); 
    }

    // Quit butonuna basıldığında çalışacak
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Oyun kapatıldı!"); // Editörde kapandığını anlamak için
    }

    // Slider kaydırıldıkça çalışacak
    public void SetSensitivity(float value)
    {
        // Değeri PlayerPrefs sistemine "MouseSensitivity" anahtar kelimesiyle kaydet
        PlayerPrefs.SetFloat("MouseSensitivity", value);
        PlayerPrefs.Save(); // Anında kaydetmesini sağla
        Debug.Log("Yeni Hassasiyet Kaydedildi: " + value);
    }
}
