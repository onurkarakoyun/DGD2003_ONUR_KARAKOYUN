using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float walkSpeed = 5f;
    public float gravity = -9.81f;

    [Header("Kamera Ayarları")]
    public float mouseSensitivity = 2f;
    public Transform playerCamera;

    private CharacterController characterController;
    private float cameraPitch = 0f;
    private float velocityY = 0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 2f);
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);
        playerCamera.localEulerAngles = Vector3.right * cameraPitch;

        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveDirection = (transform.right * moveX) + (transform.forward * moveZ);
        
        if (characterController.isGrounded && velocityY < 0)
        {
            velocityY = -2f;
        }
        velocityY += gravity * Time.deltaTime;


        Vector3 finalVelocity = (moveDirection * walkSpeed) + (Vector3.up * velocityY);
        characterController.Move(finalVelocity * Time.deltaTime);
    }
}