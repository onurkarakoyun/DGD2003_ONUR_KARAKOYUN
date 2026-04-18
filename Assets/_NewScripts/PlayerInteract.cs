using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Etkileşim Ayarları")]
    public float interactDistance = 3f;
    public Transform playerCamera;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PerformRaycast();
        }
    }

    private void PerformRaycast()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            if (hit.collider.CompareTag("Key"))
            {
                KeyPickup key = hit.collider.GetComponent<KeyPickup>();
                if (key != null)
                {
                    key.Collect();
                }
            }
            else if (hit.collider.CompareTag("Door"))
            {
                DoorController door = hit.collider.GetComponent<DoorController>();
                if (door != null)
                {
                    door.TryOpenDoor();
                }
            }
        }
    }
}