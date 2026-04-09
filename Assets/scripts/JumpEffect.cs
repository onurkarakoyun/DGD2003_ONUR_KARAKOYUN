using UnityEngine;
using DG.Tweening; // DOTween kütüphanesini kullanmak için

public class JumpEffect : MonoBehaviour
{
    [Header("Görsel Ayarlar")]
    [Tooltip("Sadece boyutu değişecek olan görsel 3D modeli buraya sürükleyin.")]
    public Transform visualModel; 

    [Header("Zıplama (Esneme) Ayarları")]
    [Tooltip("Karakter yukarı doğru uzarken ne kadar incelecek? (Varsayılan: 0.7)")]
    public float stretchXZ = 0.7f;
    [Tooltip("Karakter yukarı doğru ne kadar uzayacak? (Varsayılan: 1.4)")]
    public float stretchY = 1.4f;
    [Tooltip("Esneme animasyonu kaç saniye sürecek?")]
    public float stretchDuration = 0.2f;

    [Header("Yere Düşme (Ezilme) Ayarları")]
    [Tooltip("Karakter yere çarpınca yanlara ne kadar genişleyecek? (Varsayılan: 1.4)")]
    public float squashXZ = 1.4f;
    [Tooltip("Karakter yere çarpınca ne kadar basıklaşacak? (Varsayılan: 0.6)")]
    public float squashY = 0.6f;
    [Tooltip("Ezilme animasyonu kaç saniye sürecek?")]
    public float squashDuration = 0.15f;

    // Objenin oyun başındaki orijinal boyutunu hafızada tutacağız
    private Vector3 originalScale;

    void Start()
    {
        // Oyun başladığında görsel modelin boyutunu kaydediyoruz
        if (visualModel != null)
        {
            originalScale = visualModel.localScale;
        }
        else
        {
            Debug.LogWarning("Lütfen JumpEffect scriptine karakterin görsel modelini atayın!");
        }
    }

    /// <summary>
    /// Karakter zıpladığı anda çağrılır (Esneme)
    /// </summary>
    public void PlayJumpStretch()
    {
        if (visualModel == null) return; // Görsel yoksa hata vermemesi için koruma

        visualModel.DOKill();
        visualModel.localScale = originalScale;

        // X, Y ve Z değerlerini Inspector'dan belirlediğimiz public değişkenlerle çarpıyoruz
        visualModel.DOScale(new Vector3(originalScale.x * stretchXZ, originalScale.y * stretchY, originalScale.z * stretchXZ), stretchDuration)
            .SetLoops(2, LoopType.Yoyo) 
            .SetEase(Ease.InOutSine);   
    }

    /// <summary>
    /// Karakter yere çarptığı anda çağrılır (Ezilme)
    /// </summary>
    public void PlayLandSquash()
    {
        if (visualModel == null) return;

        visualModel.DOKill();
        visualModel.localScale = originalScale;

        // X, Y ve Z değerlerini Inspector'dan belirlediğimiz public değişkenlerle çarpıyoruz
        visualModel.DOScale(new Vector3(originalScale.x * squashXZ, originalScale.y * squashY, originalScale.z * squashXZ), squashDuration)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.OutQuad);
    }
}