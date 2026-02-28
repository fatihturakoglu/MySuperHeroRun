using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float forwardSpeed = 8f;
    [SerializeField] private float sideSpeed = 10f;
    [SerializeField] private float xLimit = 4.5f;
    [SerializeField] private float SuperSpeedMultiplier = 1.5f; // Krakterin gerçek hızını artırır stat olan hızını değil !

    [Header("Physical Changes")]
    [SerializeField] private float growthAmount = 0.15f; // Her kapıda büyüme oranı
    [SerializeField] private float maxGrowth = 2.5f;     // Maksimum büyüme sınırı

    [Header("Flying Settings")]
    [SerializeField] private float flyHeight = 2.5f;     // Yükseleceği mesafe
    [SerializeField] private float flyDuration = 1.2f;
    private float _groundY = 0f;

    [Header("Stat Settings")]
    [SerializeField] private float PowerBoostValue = 10f;
    [SerializeField] private float SpeedBoostValue = 5f;

    [Header("UI Elements")]
    [SerializeField] private GameObject textCanvas; // Kafasındaki Canvas
    [SerializeField] private TMPro.TMP_Text popupText; // TMPro kullanıyorsan
    [SerializeField] private float popupTextDuration = 1.5f; // Popup yazısının ekranda kalma süresi

    private Animator _animator;    // Animation controller'a kod ile bağlanmak için bir referans
    private string _lastMovementTrigger = "DefaultRun"; // Başlangıçta varsayılan animasyon

    private Rigidbody _rb; // Rigidbody referansı
    public HeroStatsSO myStats; // Karakterin güç, hız ve uçma yeteneği gibi istatistiklerini tutan ScriptableObject
   
    private bool _hasGameStarted = false;  // Tap to start mekanizması için bir kontrol değişkeni

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _groundY = transform.position.y; // Başlangıçta karakterin Y pozisyonunu kaydet
    }

    void Update()
    {
        // Oyun başlamadıysa ilk dokunuşu bekle
        if (!_hasGameStarted)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _hasGameStarted = true;
                _animator.SetTrigger("DefaultRun"); // Koşma animasyonunu tetikle
            }
            return; // Oyun başlamadıysa aşağıya (hareket kodlarına) geçme
        }

        // Oyun başladıysa bu iki fonksiyon her karede (frame) otomatik çalışır
        MoveForward();
        HandleSwerve();
    }

    private void MoveForward()
{
    // Vector3.forward yerine Dünya eksenindeki ileri yönü (Vector3.forward) kullanarak, 
    // objenin kendi rotasyonundan bağımsız dümdüz gitmesini sağlıyoruz.
    transform.position += Vector3.forward * (forwardSpeed * Time.deltaTime);
}  // ileri gitme hareketi

    private void HandleSwerve()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float newX = transform.position.x + mouseX * sideSpeed * Time.deltaTime;
        newX = Mathf.Clamp(newX, -xLimit, xLimit);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }   // sağ-sola gitme hareketi

    public void ShowStatPopup(string message, Color color)
    {
        // Eğer halihazırda bir yazı efekti çalışıyorsa onu durdur (çakışma olmasın)
        StopCoroutine("PopupRoutine");
        StartCoroutine(PopupRoutine(message, color));
    }  // popup text (Kraterin üstünde çıkan bilgilendirme yazısı)

    private IEnumerator PopupRoutine(string message, Color color)
    {
        popupText.text = message;
        popupText.color = color;
        textCanvas.SetActive(true);

        Vector3 startPos = new Vector3(0, 2f, 0); // Karakterin kafasına göre yerel pozisyon
        Vector3 endPos = new Vector3(0, 3.5f, 0); // Yukarı kayma mesafesi
        //float duration = 1f;
        float elapsed = 0f;

        while (elapsed < popupTextDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / popupTextDuration;

            // Yazıyı yukarı kaydır
            popupText.transform.localPosition = Vector3.Lerp(startPos, endPos, t);

            // Yazıyı yavaşça şeffaflaştır (Fade Out)
            Color c = popupText.color;
            c.a = Mathf.Lerp(1, 0, t);
            popupText.color = c;

            yield return null;
        }

        textCanvas.SetActive(false);
    }

    public void PowerCard()
    {
        // Karakterin ana objesinin ölçeğini alıyoruz
        Vector3 currentScale = transform.localScale;

        // Her yöne (X, Y, Z) eşit şekilde büyüme ekliyoruz
        Vector3 newScale = currentScale + (Vector3.one * growthAmount);

        // Sınırı aşmamasını sağla (Mathf.Clamp ile 1 ile maxGrowth arası)
        newScale.x = Mathf.Clamp(newScale.x + 0.2f, 1f, maxGrowth);
        newScale.y = Mathf.Clamp(newScale.y, 1f, maxGrowth);
        newScale.z = Mathf.Clamp(newScale.z + 0.2f, 1f, maxGrowth);

        myStats.currentStrength += PowerBoostValue; // Güç istatistiğini artır
        
        _lastMovementTrigger = "Run";
        _animator.SetTrigger("Run"); // Büyüme animasyonunu tetikle
        
        // Yeni ölçeği uygula
        transform.localScale = newScale;
        
        ShowStatPopup("STRENGTH ++", Color.blue);
        
        Debug.Log("Karakter devleşiyor! Yeni Ölçek: " + newScale.x);
    }   // Güç Kartı 

    public void NegativePowerCard()
    {
        // Karakterin ana objesinin ölçeğini alıyoruz
        Vector3 currentScale = transform.localScale;
        // Her yöne (X, Y, Z) eşit şekilde küçülme ekliyoruz
        Vector3 newScale = currentScale - (Vector3.one * growthAmount);
        // Sınırı aşmamasını sağla (Mathf.Clamp ile 0.5 ile 1 arası)
        newScale.x = Mathf.Clamp(newScale.x - 0.2f, 0.5f, 1f);
        newScale.y = Mathf.Clamp(newScale.y, 0.5f, 1f);
        newScale.z = Mathf.Clamp(newScale.z - 0.2f, 0.5f, 1f);
        myStats.currentStrength -= PowerBoostValue; // Güç istatistiğini azalt
        
        _lastMovementTrigger = "Run";
        _animator.SetTrigger("Run"); // Küçülme animasyonunu tetikle
        
        // Yeni ölçeği uygula
        transform.localScale = newScale;

        ShowStatPopup("STRENGTH --", Color.red);

        Debug.Log("Karakter küçülüyor! Yeni Ölçek: " + newScale.x);
    } // Negatif Güç Kartı 

    public void FlyingCard()
    {
        _lastMovementTrigger = "Flying";
        _animator.SetTrigger("Flying");

        if (myStats.canFly) return;

        myStats.canFly = true; // Uçma yeteneğini aç

        // 1. Yer çekimini kapat ki aşağı düşmesin
        if (_rb != null)
        {
            _rb.useGravity = false;
            _rb.velocity = Vector3.zero; // Mevcut düşme hızını sıfırla
        }

        ShowStatPopup("FLYİNG ++", Color.blue);

        StartCoroutine(SmoothFlyRoutine());
    }  // Uçma Kartı

    private IEnumerator SmoothFlyRoutine()
    {
        Vector3 startPos = transform.position;
        // HATA BURADAYDI: startPos.y yerine _groundY kullanıyoruz
        float targetY = _groundY + flyHeight;
        float elapsedTime = 0f;

        while (elapsedTime < flyDuration)
        {
            // Eğer Coroutine çalışırken biri NegativeFlyingCard alırsa yükselmeyi kes
            if (!myStats.canFly) yield break;

            elapsedTime += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsedTime / flyDuration);

            float currentY = Mathf.Lerp(startPos.y, targetY, t);
            transform.position = new Vector3(transform.position.x, currentY, transform.position.z);

            yield return null;
        }
    }

    public void NegativeFlyingCard()
    {
        // Eğer karakter zaten yerdeyse tekrar inmeye çalışmasın
        if (!myStats.canFly) return;

        myStats.canFly = false; // Uçma yeteneğini kapat

        // Animasyonu koşmaya geri döndür
        _lastMovementTrigger = "DefaultRun";
        _animator.SetTrigger(_lastMovementTrigger);

        ShowStatPopup("Flying --", Color.red);

        // Yumuşak iniş sürecini başlat
        StartCoroutine(SmoothLandRoutine());
    }// Negatif Uçma Kartı 

    private IEnumerator SmoothLandRoutine()
    {
        Vector3 startPos = transform.position;
        // Hedefimiz en başta kaydettiğimiz zemin yüksekliği (_groundY)
        float targetY = _groundY;
        float elapsedTime = 0f;
        float landDuration = flyDuration * 0.8f; // İniş, çıkıştan biraz daha hızlı olsun (daha gerçekçi)

        while (elapsedTime < landDuration)
        {
            // Eğer iniş sırasında karakter tekrar uçma kartı alırsa inişi iptal et
            if (myStats.canFly) yield break;

            elapsedTime += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsedTime / landDuration);

            float currentY = Mathf.Lerp(startPos.y, targetY, t);
            transform.position = new Vector3(transform.position.x, currentY, transform.position.z);

            yield return null;
        }

        // Tam olarak yere ulaştığında yer çekimini tekrar açabiliriz 
        // (Fizik motorunun zemini algılaması için)
        if (_rb != null)
        {
            _rb.useGravity = true;
        }

        Debug.Log("Güvenli iniş tamamlandı, karakter koşmaya devam ediyor.");
    }

    public void SuperSpeedCard()  // Hız Kartı
    {
        forwardSpeed *= SuperSpeedMultiplier; // Mevcut ileri hızını çarpanla artır
        _lastMovementTrigger = "FastRun";
        _animator.SetTrigger("FastRun");
        myStats.currentSpeed += SpeedBoostValue; // Hız istatistiğini artır
        ShowStatPopup("SPEED ++", Color.blue);
    }

    public void NegativeSpeedCard() // Negatif Hız Kartı
    {
        forwardSpeed /= SuperSpeedMultiplier;
        _lastMovementTrigger = "DefaultRun";
        _animator.SetTrigger("DefaultRun");
        myStats.currentSpeed -= SpeedBoostValue; // Hız istatistiğini artır
        ShowStatPopup("SPEED --", Color.red);
    }

    public void MartialArtCard() // Dövüş Sanatları Kartı
    {
        _animator.SetTrigger("FlyingKick");

        // Stat artışları
        myStats.currentStrength += PowerBoostValue / 2;
        myStats.currentSpeed += SpeedBoostValue / 2;

        ShowStatPopup("MARTİAL SKİLL ++", Color.blue);

        // Animasyon bittikten sonra eski duruma dönmesi için Coroutine başlat
        StartCoroutine(ReturnToLastAnimation());
    }

    public void NegativeMartialArtCard() // Dövüş Sanatları Kartı
    {
        _animator.SetTrigger("NegativeMartialArt");

        // Stat azalışları
        myStats.currentStrength -= PowerBoostValue / 2;
        myStats.currentSpeed -= SpeedBoostValue / 4;

        ShowStatPopup("MARTİAL SKİLL --", Color.red);

        // Animasyon bittikten sonra eski duruma dönmesi için Coroutine başlat
        StartCoroutine(ReturnToLastAnimation());
    }

    private IEnumerator ReturnToLastAnimation()
    {
        // Uçan tekme animasyonunun süresi kadar bekle (Örn: 0.8 saniye)
        // Buradaki süreyi animasyonunun uzunluğuna göre ayarla
        yield return new WaitForSeconds(0.8f);
    
        _animator.SetTrigger(_lastMovementTrigger);  // Tekme bittiğinde, hafızadaki son hareket animasyonunu tekrar çalıştır
    }

}