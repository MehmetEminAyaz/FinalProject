using UnityEngine;

public class CharacterControllerScript : MonoBehaviour
{
    public float walkSpeed = 2.0f; // Yürüme hýzý
    public float runSpeed = 5.0f; // Koþma hýzý
    public float turnSpeed = 360.0f; // Dönüþ hýzý
    private Animator animator; // Animator bileþeni
    private CharacterController characterController; // Karakter kontrol bileþeni

    void Start()
    {
        animator = GetComponent<Animator>(); // Animator'ý al
        characterController = GetComponent<CharacterController>(); // CharacterController'ý al
    }

    void Update()
    {
        // Hareket yönünü hesapla
        float horizontal = Input.GetAxis("Horizontal"); // A-D veya Sol-Sað
        float vertical = Input.GetAxis("Vertical"); // W-S veya Yukarý-Aþaðý
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        // Shift'e basýlý olup olmadýðýný kontrol et
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        // Hýz ayarý
        float speed = isRunning ? runSpeed : walkSpeed;

        if (direction.magnitude >= 0.1f)
        {
            // Yönü hesapla
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSpeed, 0.1f);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            // Hareketi uygula
            Vector3 move = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            characterController.Move(move * speed * Time.deltaTime);

            // Animasyonlar
            animator.SetBool("isWalking", !isRunning); // Yürüme animasyonu
            animator.SetBool("isRunning", isRunning); // Koþma animasyonu
        }
        else
        {
            // Hareket yoksa idle
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
    }
}
