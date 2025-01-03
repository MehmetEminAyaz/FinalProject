using UnityEngine;

public class CharacterControllerScript : MonoBehaviour
{
    public float walkSpeed = 2.0f; // Y�r�me h�z�
    public float runSpeed = 5.0f; // Ko�ma h�z�
    public float turnSpeed = 360.0f; // D�n�� h�z�
    private Animator animator; // Animator bile�eni
    private CharacterController characterController; // Karakter kontrol bile�eni

    void Start()
    {
        animator = GetComponent<Animator>(); // Animator'� al
        characterController = GetComponent<CharacterController>(); // CharacterController'� al
    }

    void Update()
    {
        // Hareket y�n�n� hesapla
        float horizontal = Input.GetAxis("Horizontal"); // A-D veya Sol-Sa�
        float vertical = Input.GetAxis("Vertical"); // W-S veya Yukar�-A�a��
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        // Shift'e bas�l� olup olmad���n� kontrol et
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        // H�z ayar�
        float speed = isRunning ? runSpeed : walkSpeed;

        if (direction.magnitude >= 0.1f)
        {
            // Y�n� hesapla
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSpeed, 0.1f);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            // Hareketi uygula
            Vector3 move = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            characterController.Move(move * speed * Time.deltaTime);

            // Animasyonlar
            animator.SetBool("isWalking", !isRunning); // Y�r�me animasyonu
            animator.SetBool("isRunning", isRunning); // Ko�ma animasyonu
        }
        else
        {
            // Hareket yoksa idle
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
    }
}
