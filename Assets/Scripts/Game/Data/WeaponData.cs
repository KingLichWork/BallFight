using System;
using UnityEngine;

public enum WeaponType
{
    /// <summary>Меч, копьё, молот, коса — вращается, бьёт при касании.</summary>
    Melee,

    /// <summary>Кинжал — вращается очень быстро, короткий, бьёт при касании.</summary>
    Spin,

    /// <summary>Щит — широкий, блокирует и растёт.</summary>
    Shield,

    /// <summary>Лук — вращается и периодически стреляет снарядом в направлении кончика.</summary>
    Ranged
}

[Serializable]
[CreateAssetMenu(menuName = "Data/WeaponData", fileName = "WeaponData")]
public class WeaponData : ScriptableObject
{
    public GameObject weaponPrefab;
    public WeaponScaling scaling;

    public string weaponName = "Sword";
    public Sprite weaponSprite;
    public WeaponType behaviourType = WeaponType.Melee;

    [Header("Базовые параметры")]
    [Min(0)] public float baseDamage = 10f;
    [Min(0.1f)] public float baseLength = 1.2f;
    [Min(10)] public float baseRotationSpeed = 180f;
    [Min(0.05f)] public float baseWidth = 0.15f;

    public bool canParry = true;
    public bool ignoresParry = false;
    [Min(0)] public float parryStunDuration = 0.3f;

    [Header("Ranged")]
    public GameObject projectilePrefab;
    [Min(0.1f)] public float fireInterval = 1.5f;

    public WeaponRuntimeStats CreateRuntimeStats() => new WeaponRuntimeStats(this);
}
