using UnityEngine;

[CreateAssetMenu(menuName = "AI/Monster Settings")]
public class MonsterSettings : ScriptableObject
{
    public float visionRange = 10f;
    public float visionAngle = 120f;
    public float alertRadius = 8f;
    public float identificationDelay = 2f;
}

