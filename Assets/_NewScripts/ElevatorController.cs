using UnityEngine;
using UnityEngine.SceneManagement;

public class ElevatorController : MonoBehaviour
{
    [Header("Asansör Ayarları")]
    public int requiredKeycardID = 3; 

    [Header("Kapı Animasyonu")]
    public Transform elevatorDoor; // Aşağı inecek kapı objesi
    public float openDistance = 3f; // Kapı kaç metre aşağı inecek?
    public float doorSpeed = 2f;    // Kapının inme hızı
    private Vector3 doorTargetPos;  // Kapının varacağı son nokta

    [Header("Karakter Sinematiği")]
    public Transform playerTransform;      // Oyuncumuz
    public Transform insideElevatorTarget; // Oyuncunun yürüyeceği hedef nokta
    public float playerWalkSpeed = 2f;
    public Animator playerAnimator;
         // Karakterin yürüme hızı

    private bool isUnlocked = false;
    private bool isEscaping = false;
    private bool isDoorOpening = false;
    private bool isPlayerWalking = false;

    void Start()
    {
        // Oyun başında, kapının şu anki konumundan 'openDistance' kadar aşağsını hedef olarak belirle
        if (elevatorDoor != null)
        {
            doorTargetPos = elevatorDoor.localPosition - new Vector3(0, openDistance, 0);
        }
    }

    void OnEnable() { KeyPickup.OnKeyCollected.AddListener(CheckKeycard); }
    void OnDisable() { KeyPickup.OnKeyCollected.RemoveListener(CheckKeycard); }

    private void CheckKeycard(int collectedKeyID)
    {
        if (collectedKeyID == requiredKeycardID)
        {
            isUnlocked = true;
            Debug.Log("Asansör kartı alındı!");
        }
    }

    public void Interact()
    {
        if (isEscaping) return;

        if (isUnlocked)
        {
            isEscaping = true;
            Debug.Log("Kaçış sinematiği başlıyor! Kapı açılıyor ve karakter yürüyor...");

            // 1. Oyuncunun kontrollerini kapat (Artık WASD ve Fare çalışmasın)
            FPSController fps = playerTransform.GetComponent<FPSController>();
            if (fps != null) fps.enabled = false;

            // Karakteri kodla hareket ettirebilmek için fizik engelleyicisini (CharacterController) kapatıyoruz
            CharacterController cc = playerTransform.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false; 

            // 2. Animasyonları (Update içindeki kodları) tetikle
            isDoorOpening = true;
            isPlayerWalking = true;
            if (playerAnimator != null) playerAnimator.SetBool("isWalking", true);

            // 3. Çıkış Kamerasına Geçiş Yap
            CameraManager camManager = Object.FindFirstObjectByType<CameraManager>();
            if (camManager != null) camManager.ActivateExitCamera();

            // 4. 4.5 saniye boyunca bu sinematiği izlet ve sonra oyunu bitir
            Invoke("EndGame", 4.5f);
        }
    }

    void Update()
    {
        // KAPIYI AŞAĞI İNDİRME EFEKTİ (Lerp ile yumuşakça)
        if (isDoorOpening && elevatorDoor != null)
        {
            elevatorDoor.localPosition = Vector3.Lerp(elevatorDoor.localPosition, doorTargetPos, Time.deltaTime * doorSpeed);
        }

        // KARAKTERİ ASANSÖRÜN İÇİNE YÜRÜTME EFEKTİ (MoveTowards ile sabit hızda)
        if (isPlayerWalking && playerTransform != null && insideElevatorTarget != null)
        {
            playerTransform.position = Vector3.MoveTowards(playerTransform.position, insideElevatorTarget.position, Time.deltaTime * playerWalkSpeed);
        }
    }

    private void EndGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}