using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI; // Arayüz (UI) elemanlarını kullanmak için ekledik

public class ComputerFix : MonoBehaviour
{
    [Header("UI Ayarları")]
    [Tooltip("Bilgisayarın üstündeki 'E - Tamir Et' yazısı")]
    public GameObject repairTextObj; 
    
    [Tooltip("Dolum barının tamamı (Arkaplanı içeren ana obje)")]
    public GameObject progressBarContainer; 
    
    [Tooltip("İçi dolacak olan renkli bar (Image)")]
    public Image progressBarFill; 

    [Header("Tamir Ayarları")]
    [Tooltip("Tamirin tamamlanması için kaç saniye E'ye basılı tutmalı?")]
    public float repairTime = 3f; 
    private float currentRepairProgress = 0f; // O anki dolum miktarı

    [Header("Durum")]
    public bool isBroken = false; 
    
    public UnityEvent OnFixed;

    private bool isPlayerInRange = false; 

    void Start()
    {
        // Oyun başında UI elemanlarını gizle ve barı sıfırla
        if (repairTextObj != null) repairTextObj.SetActive(false);
        if (progressBarContainer != null) progressBarContainer.SetActive(false);
        if (progressBarFill != null) progressBarFill.fillAmount = 0f;
    }

    void Update()
    {
        if (isBroken && isPlayerInRange)
        {
            // Eğer oyuncu E tuşuna BASILI TUTUYORSA
            if (Input.GetKey(KeyCode.E))
            {
                // "E - Tamir Et" yazısını gizle, Dolum barını göster
                if (repairTextObj != null) repairTextObj.SetActive(false);
                if (progressBarContainer != null) progressBarContainer.SetActive(true);

                // Süreyi artır ve barı doldur
                currentRepairProgress += Time.deltaTime;
                
                if (progressBarFill != null) 
                {
                    // Barın doluluk oranını (0 ile 1 arası) hesapla
                    progressBarFill.fillAmount = currentRepairProgress / repairTime;
                }

                // Bar tamamen dolduysa tamiri bitir!
                if (currentRepairProgress >= repairTime)
                {
                    FixComputer();
                }
            }
            else // E tuşunu BIRAKIRSA veya basmıyorsa
            {
                // İlerlemeyi sıfırla, barı gizle, yazıyı tekrar göster
                currentRepairProgress = 0f;
                if (progressBarFill != null) progressBarFill.fillAmount = 0f;
                
                if (progressBarContainer != null) progressBarContainer.SetActive(false);
                if (repairTextObj != null) repairTextObj.SetActive(true);
            }
        }
    }

    void FixComputer()
    {
        isBroken = false;
        currentRepairProgress = 0f; // İlerlemeyi sıfırla
        
        // Tüm UI elemanlarını gizle
        if (repairTextObj != null) repairTextObj.SetActive(false);
        if (progressBarContainer != null) progressBarContainer.SetActive(false);
        
        Debug.Log(gameObject.name + " Tamir Edildi!");
        
        OnFixed.Invoke(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isBroken)
        {
            isPlayerInRange = true;
            if (repairTextObj != null) repairTextObj.SetActive(true); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            currentRepairProgress = 0f; // Oyuncu uzaklaşırsa ilerleme sıfırlanır
            
            if (repairTextObj != null) repairTextObj.SetActive(false); 
            if (progressBarContainer != null) progressBarContainer.SetActive(false);
            if (progressBarFill != null) progressBarFill.fillAmount = 0f;
        }
    }
}