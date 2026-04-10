using System.Collections;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f; 
    public float jumpForce = 5f;
    public float mouseSensitivty = 200f;
    public Transform cameraPivot;
    
    [Header("Zemin Kontrolü (Ground Check)")]
    public Transform groundCheck; 
    public float groundDistance = 0.2f; 
    public LayerMask groundMask; 
    private bool isGrounded; 

    [Header("Animasyon Ayarları")]
    public Animator animator; 
    public KeyCode sprintKey = KeyCode.LeftShift; 

    private Rigidbody rb;
    private float xRotation = 0f;

    // Efekt değişkenleri
    private JumpEffect jumpEffect; 
    private bool wasInAir = false; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpEffect = GetComponent<JumpEffect>(); 

        if (animator == null) animator = GetComponentInChildren<Animator>();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        MouseLook();
    }

    void Update()
    {
        CheckIfGrounded();
        
        MoveAndAnimate(); 
        Jump();
        CheckLandingEffect(); 
        
        // Düşme ve zıplama geçişleri için dikey hızı ve yer bilgisini Animator'a gönderiyoruz
        if (animator != null)
        {
            animator.SetFloat("YVelocity", rb.linearVelocity.y);
            animator.SetBool("IsGrounded", isGrounded);
        }
    }

    void MoveAndAnimate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        bool isRunning = Input.GetKey(sprintKey);
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        Vector3 movement = move * currentSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);

        if (animator != null)
        {
            float animSpeedMultiplier = isRunning ? 2f : 1f; 
            animator.SetFloat("X", horizontal * animSpeedMultiplier, 0.1f, Time.deltaTime);
            animator.SetFloat("Y", vertical * animSpeedMultiplier, 0.1f, Time.deltaTime);
        }
    }

    void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivty * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivty * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void CheckIfGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            if (jumpEffect != null) jumpEffect.PlayJumpStretch();

            if (animator != null) animator.SetTrigger("Jump");
        }
    }

    void CheckLandingEffect()
    {
        // Karakter havadan yere TIK diye çarptığı an
        if (wasInAir && isGrounded)
        {
            // 1. Görsel ezilme efektini çalıştır
            if (jumpEffect != null) jumpEffect.PlayLandSquash();

            // 2. Animator'a "Yere İndik (Land)" tetikleyicisini gönder
            if (animator != null) animator.SetTrigger("Land");
        }

        wasInAir = !isGrounded; 
    }
}