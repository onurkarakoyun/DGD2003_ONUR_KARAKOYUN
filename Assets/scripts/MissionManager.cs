using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [Header("Görev Sırası")]
    [Tooltip("Tamir edilecek bilgisayarları buraya sürükleyin. Oyun başladığında sıraları otomatik olarak rastgele karışacaktır.")]
    public ComputerFix[] computersToFix;
    
    private int currentIndex = 0; // Şu an kaçıncı bilgisayardayız?

    [Header("Yönlendirme Oku")]
    public ArrowPointer arrowPointer; // Oyuncunun tepesindeki ok

    void Start()
    {
        // 1. ADIM: Oyun başladığında bilgisayar listesini rastgele karıştır!
        ShuffleComputers();

        // 2. ADIM: Tüm bilgisayarları sağlam yap ve tamir olayını dinlemeye başla
        foreach (var pc in computersToFix)
        {
            pc.isBroken = false;
            // Bilgisayar tamir edildiğinde NextComputer fonksiyonunu çağır
            pc.OnFixed.AddListener(NextComputer);
        }

        // 3. ADIM: Listedeki ilk (artık rastgele olan) bilgisayarı boz ve görevi başlat
        ActivateCurrentComputer();
    }

    /// <summary>
    /// Dizideki elemanların yerlerini rastgele değiştirir (Fisher-Yates Shuffle algoritması)
    /// </summary>
    void ShuffleComputers()
    {
        for (int i = 0; i < computersToFix.Length; i++)
        {
            // O anki sıradaki bilgisayar ile dizinin geri kalanından rastgele seçilen bir bilgisayarın yerini değiştir
            int randomIndex = Random.Range(i, computersToFix.Length);
            
            ComputerFix temp = computersToFix[i];
            computersToFix[i] = computersToFix[randomIndex];
            computersToFix[randomIndex] = temp;
        }
    }

    void ActivateCurrentComputer()
    {
        // Eğer hala tamir edilecek bilgisayar varsa
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
            // Liste bittiyse oyunu kazandık!
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