using UnityEngine;

public enum ScalingType
{
    MeleeDamageOnHit,
    MeleeDamageAndLengthOnHit,
    MeleeSpeedScalesDamage,
    MeleePoisonOnHit,
    SpinDamageAndSpeedOnHit,
    ShieldWidthOnParry,
    RangedFireRateOnHit,
}

[System.Serializable]
public class WeaponRuntimeStats
{
    public float damage;
    public float length;
    public float rotationSpeed;
    public float width;
    public float fireInterval;
    public int hitCount;
    public int parryCount;

    public WeaponRuntimeStats(WeaponData data)
    {
        damage = data.baseDamage;
        length = data.baseLength;
        rotationSpeed = data.baseRotationSpeed;
        width = data.baseWidth;
        fireInterval = data.fireInterval;
    }
}

[CreateAssetMenu(menuName = "WBB/WeaponScaling", fileName = "Scaling_New")]
public class WeaponScaling : ScriptableObject
{
    public ScalingType type;

    [Header("MeleeDamageOnHit")]
    [Min(0)] public float damagePerHit = 1f;

    [Header("MeleeDamageAndLength / MeleePoisonOnHit")]
    [Min(0)] public float lengthPerHit = 0.5f;

    [Header("MeleeSpeedScalesDamage")]
    [Min(0)] public float speedPerHit = 30f;
    [Min(0)] public float maxSpeed = 720f;
    [Min(0)] public float damagePerSpeed = 0.05f;

    [Header("MeleePoisonOnHit")]
    [Min(0)] public float poisonStacksPerHit = 1f;
    [Min(0)] public float poisonDamagePerStack = 5f;
    [Min(0)] public float poisonTickInterval = 0.5f;

    [Header("DaggerDamageAndSpeedOnHit")]
    [Min(0)] public float daggerDamagePerHit = 0.5f;
    [Min(0)] public float daggerSpeedPerHit = 20f;
    [Min(0)] public float daggerMaxSpeed = 1080f;

    [Header("ShieldWidthOnParry")]
    [Min(0)] public float widthPerParry = 0.2f;
    [Min(0)] public float maxWidth = 3f;

    [Header("RangedFireRateOnHit")]
    [Tooltip("Уменьшение интервала стрельбы за каждое попадание снаряда")]
    [Min(0)] public float fireIntervalReduction = 0.1f;
    [Tooltip("Минимальный интервал стрельбы")]
    [Min(0.1f)] public float minFireInterval = 0.4f;

    public float OnHit(WeaponRuntimeStats stats)
    {
        stats.hitCount++;
        float poison = 0f;

        Debug.Log("Scaling");

        switch (type)
        {
            case ScalingType.MeleeDamageOnHit:
                stats.damage += damagePerHit;
                break;

            case ScalingType.MeleeDamageAndLengthOnHit:
                stats.damage += damagePerHit;
                stats.length += lengthPerHit;
                break;

            case ScalingType.MeleeSpeedScalesDamage:
                stats.rotationSpeed = Mathf.Min(stats.rotationSpeed + speedPerHit, maxSpeed);
                stats.damage = stats.rotationSpeed * damagePerSpeed;
                break;

            case ScalingType.MeleePoisonOnHit:
                poison = poisonStacksPerHit;
                break;

            case ScalingType.SpinDamageAndSpeedOnHit:
                stats.damage += daggerDamagePerHit;
                stats.rotationSpeed = Mathf.Min(stats.rotationSpeed + daggerSpeedPerHit, daggerMaxSpeed);
                break;

            case ScalingType.RangedFireRateOnHit:
                stats.fireInterval = Mathf.Max(stats.fireInterval - fireIntervalReduction, minFireInterval);
                break;
        }

        return poison;
    }

    public void OnParry(WeaponRuntimeStats stats)
    {
        stats.parryCount++;

        if (type == ScalingType.ShieldWidthOnParry)
            stats.width = Mathf.Min(stats.width + widthPerParry, maxWidth);
    }
}
