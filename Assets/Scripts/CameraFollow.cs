using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target; // Takip edilecek karakter
    [SerializeField] private Vector3 offset;   // Karakter ile kamera arasındaki mesafe

    [Header("Smooth Settings")]
    [SerializeField] private float smoothSpeed = 5f; // Takip yumuşaklığı

    void LateUpdate()
    {
        if (target == null) return;

        // Kameranın gitmesi gereken ideal pozisyonu hesapla
        Vector3 desiredPosition = target.position + offset;

        // Kamerayı mevcut yerinden, gitmesi gereken yere yumuşak bir şekilde kaydır
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Kameranın pozisyonunu güncelle
        transform.position = smoothedPosition;
    }
}