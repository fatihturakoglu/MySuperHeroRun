using UnityEngine;

public class FlyingGate : MonoBehaviour
{
    private bool _isUsed = false;

    private void OnTriggerEnter(Collider other)
    {
        // Kapı zaten kullanıldıysa işlem yapma
        if (_isUsed) return;

        // Karakteri algıla (Senin PlayerController script'ini arıyoruz)
        PlayerController player = other.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            _isUsed = true; // Kapıyı kilitler

            // SENİN METHODUNU ÇAĞIRIYORUZ:
            player.FlyingCard();

            Debug.Log("Uçuş kapısından geçildi, karakter scriptindeki FlyingCard tetiklendi.");

            // Kapının içinden geçilebilir olması için yok etmiyoruz
        }
    }
}