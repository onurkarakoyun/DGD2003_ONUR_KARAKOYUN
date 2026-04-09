using System.Collections;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float speed = 5f;
    public float jumpForce = 5f;
    public float mouseSensitivty = 200f;
    public Transform cameraPivot;
    
    [Header("Zemin Kontrolü (Ground Check)")]
    public Transform groundCheck; // Karakterin ayaklarındaki boş obje
    public float groundDistance = 0.2f; // Zemin kontrol küresinin yarıçapı
    public LayerMask groundMask; // Hangi katmanların "Zemin" sayılacağı
    private bool isGrounded; // Yerde miyiz?

    private Rigidbody rb;
    private float xRotation = 0f;

    // Efekt değişkenleri
    private JumpEffect jumpEffect; 
    private bool wasInAir = false; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpEffect = GetComponent<JumpEffect>(); 
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        MouseLook();
    }

    void Update()
    {
        // 1. Önce yerde miyiz onu kontrol et
        CheckIfGrounded();
        
        // 2. Hareket, zıplama ve yere düşme efektini çalıştır
        Move();
        Jump();
        CheckLandingEffect(); 
    }

    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        Vector3 movement = move * speed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);
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
        // Karakterin ayaklarında yarattığımız noktanın etrafında bir küre oluşturur.
        // Bu küre "groundMask" olarak belirlediğimiz katmanlardan birine değiyorsa "true" döner.
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    void Jump()
    {
        // Artık hıza değil, sanal küremizin yere değip değmediğine (isGrounded) bakıyoruz
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            if (jumpEffect != null)
            {
                jumpEffect.PlayJumpStretch();
            }
        }
    }

    void CheckLandingEffect()
    {
        // Eğer geçen karede havadaysak ve ŞU AN yerdeysek (CheckSphere sayesinde kesin biliyoruz)
        if (wasInAir && isGrounded)
        {
            if (jumpEffect != null)
            {
                jumpEffect.PlayLandSquash();
            }
        }

        // Bir sonraki kare için havada olma durumumuzu güncelliyoruz.
        wasInAir = !isGrounded; 
    }
}