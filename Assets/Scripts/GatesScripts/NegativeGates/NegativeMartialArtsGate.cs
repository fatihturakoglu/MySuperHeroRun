using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NegativeMartialArtsGate : MonoBehaviour
{
    private bool _isUsed = false;

    private void OnTriggerEnter(Collider other)
    {
        
        if (_isUsed == true) return;

        PlayerController player = other.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            _isUsed = true; // Kapıyı kilitler

            // SENİN METHODUNU ÇAĞIRIYORUZ:
            player.NegativeMartialArtCard();

            Debug.Log("Speed kapısından geçildi, karakter scriptindeki SuperSpeedCard tetiklendi.");

            // Kapının içinden geçilebilir olması için yok etmiyoruz
        }
    }
}
