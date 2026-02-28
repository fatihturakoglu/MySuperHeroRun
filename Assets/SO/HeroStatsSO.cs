using UnityEngine;

[CreateAssetMenu(fileName = "HeroStats", menuName = "ScriptableObjects/HeroStats")]
public class HeroStatsSO : ScriptableObject
{
    public float currentStrength;
    public bool canFly;
    public float currentSpeed;

    public void ResetStats()
    {
        currentStrength = 1f;
        canFly = false;
        currentSpeed = 1f;
    }
}