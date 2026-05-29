using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Etkileşim Ayarları")]
    public float interactDistance = 3f; 
    public float aimThickness = 0.5f; // YENİ: Işının kalınlığı (Küçük objeleri vurmayı çok kolaylaştırır)
    public Transform playerCamera;      

    private GameObject currentTarget; // Şu an baktığımız obje

    void Update()
    {
        // Her karede nereye baktığımızı kontrol et (Yazıyı aç/kapat yapmak için)
        CheckForInteractable(); 

        // Eğer bir şeye bakıyorsak ve E'ye basıldıysa
        if (Input.GetKeyDown(KeyCode.E) && currentTarget != null)
        {
            InteractWithTarget();
        }
    }

    private void CheckForInteractable()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        // SphereCast, Raycast'in kalın (silindir) versiyonudur. 'aimThickness' yarıçapıdır.
        if (Physics.SphereCast(ray, aimThickness, out hit, interactDistance))
        {
            GameObject hitObject = hit.collider.gameObject;

            // Eğer baktığımız şey etkileşimliyse
            if (hitObject.CompareTag("Key") || hitObject.CompareTag("Door") || hitObject.CompareTag("Fuse") || hitObject.CompareTag("Panel") || hitObject.CompareTag("Elevator"))
            {
                // Eğer farklı yeni bir objeye bakmaya başladıysak
                if (currentTarget != hitObject)
                {
                    HideUI(); // Eskinin yazısını kapat
                    currentTarget = hitObject;
                    ShowUI(); // Yeninin yazısını aç
                }
                return; // Bulduk, fonksiyondan çık
            }
        }

        // Eğer SphereCast hiçbir şeye vurmadıysa (boşluğa bakıyorsak)
        HideUI();
        currentTarget = null;
    }

    private void InteractWithTarget()
    {
        if (currentTarget.CompareTag("Key"))
        {
            KeyPickup key = currentTarget.GetComponent<KeyPickup>();
            if (key != null) key.Collect();
        }
        else if (currentTarget.CompareTag("Door"))
        {
            DoorController door = currentTarget.GetComponent<DoorController>();
            if (door != null) door.TryOpenDoor();
        }
        else if (currentTarget.CompareTag("Fuse"))
        {
            FusePickup fuse = currentTarget.GetComponent<FusePickup>();
            if (fuse != null) fuse.Collect();
        }
        else if (currentTarget.CompareTag("Panel"))
        {
            ElectricalPanel panel = currentTarget.GetComponent<ElectricalPanel>();
            if (panel != null) panel.Interact();
        }
        else if (currentTarget.CompareTag("Elevator"))
        {
            ElevatorController elevator = currentTarget.GetComponent<ElevatorController>();
            if (elevator != null) elevator.Interact();
        }
        
        // Eşya alındıktan sonra yok olacağı için UI'ı ve hedefi temizle
        HideUI();
        currentTarget = null;
    }

    // Seçilen hedefin içindeki 'HoverUI' adlı objeyi bulup görünür yapar
    private void ShowUI()
    {
        if (currentTarget != null)
        {
            Transform ui = currentTarget.transform.Find("HoverUI");
            if (ui != null) ui.gameObject.SetActive(true);
        }
    }

    // Seçilen hedefin yazısını gizler
    private void HideUI()
    {
        if (currentTarget != null)
        {
            Transform ui = currentTarget.transform.Find("HoverUI");
            if (ui != null) ui.gameObject.SetActive(false);
        }
    }
}