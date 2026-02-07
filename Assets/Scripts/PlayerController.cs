using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float forwardSpeed = 8f;
    [SerializeField] private float sideSpeed = 10f;
    [SerializeField] private float xLimit = 4.5f;

    // Artık _isMoving değişkenine ihtiyacımız yok çünkü hep hareket edeceğiz
    // Ancak oyunun başında bir "Dokun ve Başla" mekaniği istersen diye bir kontrol ekleyebiliriz
    private bool _hasGameStarted = false;

    void Update()
    {
        // Oyun başlamadıysa ilk dokunuşu bekle
        if (!_hasGameStarted)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _hasGameStarted = true;
            }
            return; // Oyun başlamadıysa aşağıya (hareket kodlarına) geçme
        }

        // Oyun başladıysa bu iki fonksiyon her karede (frame) otomatik çalışır
        MoveForward();
        HandleSwerve();
    }

    private void MoveForward()
    {
        // Artık bir şarta bağlı değil, her saniye ileri gider
        transform.Translate(Vector3.forward * (forwardSpeed * Time.deltaTime));
    }

    private void HandleSwerve()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float newX = transform.position.x + mouseX * sideSpeed * Time.deltaTime;
        newX = Mathf.Clamp(newX, -xLimit, xLimit);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}