using UnityEngine;

public class TPSCameraWithCollision : MonoBehaviour
{
    public Transform target; // Takip edilecek nesne (karakter)
    public float distance = 5.0f; // Kameranýn karaktere olan uzaklýðý
    public float height = 2.0f; // Kameranýn karaktere olan yüksekliði
    public float rotationSpeed = 5.0f; // Kamera dönüþ hýzý
    public float collisionOffset = 0.2f; // Çarpýþmada kameranýn engelden ne kadar uzak duracaðý
    public LayerMask collisionLayers; // Çarpýþmayý kontrol edeceðimiz katmanlar (örn: Duvarlar)

    private float currentRotationAngle;

    void LateUpdate()
    {
        if (!target) return;

        // Fare giriþlerini al
        float horizontalInput = Input.GetAxis("Mouse X");

        // Kameranýn karakter etrafýnda dönmesini saðla
        currentRotationAngle += horizontalInput * rotationSpeed;
        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Kameranýn hedef pozisyonunu hesapla
        Vector3 targetPosition = target.position + Vector3.up * height; // Karakterin yüksekliðini ekle
        Vector3 desiredCameraPosition = targetPosition - currentRotation * Vector3.forward * distance;

        // Raycast ile çarpýþma kontrolü yap
        RaycastHit hit;
        if (Physics.Linecast(targetPosition, desiredCameraPosition, out hit, collisionLayers))
        {
            // Eðer engel varsa, kamerayý engelin hemen önüne yerleþtir
            desiredCameraPosition = hit.point + hit.normal * collisionOffset;
        }

        // Kameranýn pozisyonunu güncelle
        transform.position = desiredCameraPosition;

        // Kamerayý karaktere baktýr
        transform.LookAt(target);
    }
}
