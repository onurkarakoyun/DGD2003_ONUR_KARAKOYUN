using System.Collections;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f; 
    public float mouseSensitivty = 200f;
    public Transform cameraPivot;
    
    [Header("Zıplama Ayarları")]
    public float jumpForce = 5f;
    public int maxJumps = 2; 
    private int jumpCount = 0; 

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

        if (isGrounded)
        {
            jumpCount = 0;
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || jumpCount < maxJumps))
        {
            if (!isGrounded)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            }

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpCount++; 

            if (jumpEffect != null) jumpEffect.PlayJumpStretch();

            if (animator != null) 
            {
                animator.SetTrigger("Jump");
            }
        }
    }

    void CheckLandingEffect()
    {
        if (wasInAir && isGrounded)
        {
            if (jumpEffect != null) jumpEffect.PlayLandSquash();
            
            if (animator != null) 
            {
                // ÇÖZÜM BURADA: Yere değdiğimiz an, önceden basılmış ama çalışmamış zıplama komutlarını siliyoruz.
                animator.ResetTrigger("Jump"); 
                animator.SetTrigger("Land");
            }
        }

        wasInAir = !isGrounded; 
    }
}