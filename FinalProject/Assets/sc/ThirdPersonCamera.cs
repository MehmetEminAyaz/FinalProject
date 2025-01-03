using UnityEngine;

public class TPSCameraWithCollision : MonoBehaviour
{
    public Transform target; // Takip edilecek nesne (karakter)
    public float distance = 5.0f; // Kameran�n karaktere olan uzakl���
    public float height = 2.0f; // Kameran�n karaktere olan y�ksekli�i
    public float rotationSpeed = 5.0f; // Kamera d�n�� h�z�
    public float collisionOffset = 0.2f; // �arp��mada kameran�n engelden ne kadar uzak duraca��
    public LayerMask collisionLayers; // �arp��may� kontrol edece�imiz katmanlar (�rn: Duvarlar)

    private float currentRotationAngle;

    void LateUpdate()
    {
        if (!target) return;

        // Fare giri�lerini al
        float horizontalInput = Input.GetAxis("Mouse X");

        // Kameran�n karakter etraf�nda d�nmesini sa�la
        currentRotationAngle += horizontalInput * rotationSpeed;
        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Kameran�n hedef pozisyonunu hesapla
        Vector3 targetPosition = target.position + Vector3.up * height; // Karakterin y�ksekli�ini ekle
        Vector3 desiredCameraPosition = targetPosition - currentRotation * Vector3.forward * distance;

        // Raycast ile �arp��ma kontrol� yap
        RaycastHit hit;
        if (Physics.Linecast(targetPosition, desiredCameraPosition, out hit, collisionLayers))
        {
            // E�er engel varsa, kameray� engelin hemen �n�ne yerle�tir
            desiredCameraPosition = hit.point + hit.normal * collisionOffset;
        }

        // Kameran�n pozisyonunu g�ncelle
        transform.position = desiredCameraPosition;

        // Kameray� karaktere bakt�r
        transform.LookAt(target);
    }
}
