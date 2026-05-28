using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Transform mainCamera;

    void Start()
    {
        // Sahnedeki ana kamerayı otomatik bul
        mainCamera = Camera.main.transform;
    }

    void Update()
    {
        // Yazıyı her karede kameraya doğru çevir
        if (mainCamera != null)
        {
            transform.LookAt(transform.position + mainCamera.rotation * Vector3.forward, mainCamera.rotation * Vector3.up);
        }
    }
}