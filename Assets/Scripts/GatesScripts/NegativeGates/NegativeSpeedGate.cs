using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NegativeSpeedGate : MonoBehaviour
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
            player.NegativeSpeedCard();

            Debug.Log("Speed kapısından geçildi, karakter scriptindeki SuperSpeedCard tetiklendi.");

            // Kapının içinden geçilebilir olması için yok etmiyoruz
        }
    }

}
