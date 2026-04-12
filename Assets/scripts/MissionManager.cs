using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [Header("Görev Sırası")]
    [Tooltip("Tamir edilecek bilgisayarları sırasıyla buraya sürükleyin")]
    public ComputerFix[] computersToFix;
    
    private int currentIndex = 0; // Şu an kaçıncı bilgisayardayız?

    [Header("Yönlendirme Oku")]
    public ArrowPointer arrowPointer; // Oyuncunun tepesindeki ok

    void Start()
    {
        // Oyun başında hepsini sağlam yap
        foreach (var pc in computersToFix)
        {
            pc.isBroken = false;
            // Bilgisayar tamir edildiğinde çalışacak fonksiyonu otomatik bağlıyoruz
            pc.OnFixed.AddListener(NextComputer);
        }

        // İlk bilgisayarı boz ve görevi başlat
        ActivateCurrentComputer();
    }

    void ActivateCurrentComputer()
    {
        if (currentIndex < computersToFix.Length)
        {
            // Sıradaki bilgisayarı bozuk hale getir
            computersToFix[currentIndex].isBroken = true;
            
            // Oku yeni bilgisayara yönlendir
            if (arrowPointer != null)
            {
                arrowPointer.SetTarget(computersToFix[currentIndex].transform);
            }
        }
        else
        {
            Debug.Log("TEBRİKLER! TÜM BİLGİSAYARLAR TAMİR EDİLDİ!");
            if (arrowPointer != null) arrowPointer.gameObject.SetActive(false); // Oku gizle
        }
    }

    public void NextComputer()
    {
        currentIndex++; // Bir sonrakine geç
        ActivateCurrentComputer(); // Onu aktifleştir
    }
}