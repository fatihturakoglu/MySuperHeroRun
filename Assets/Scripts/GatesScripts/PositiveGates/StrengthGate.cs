using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthGate : MonoBehaviour
{
    private bool _isUsed = false;

    private void OnTriggerEnter(Collider other)
    {
        // Kapı zaten kullanıldıysa hiçbir şey yapma
        if (_isUsed) return;

        // Çarpan objenin kendisinde veya üstünde PlayerController ara
        PlayerController player = other.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            _isUsed = true; // Kapıyı kilitler, artık bu kapı tekrar güç vermez
            player.PowerCard();

            // OPSİYONEL: Kapı kullanıldığında rengini biraz soldurabilirsin
            // Böylece oyuncu kapının "boş" olduğunu anlar.
            Debug.Log("Güç kapısından geçildi, karakter büyütüldü.");
        }
    }
}
