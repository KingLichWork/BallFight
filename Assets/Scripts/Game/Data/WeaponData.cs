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
    [Header("Идентификация")]
    public string weaponName = "Sword";
    public Color ballColor = Color.gray;
    public Sprite weaponSprite;
    public WeaponType behaviourType = WeaponType.Melee;

    [Header("Базовые параметры")]
    [Min(0)] public float baseDamage = 10f;
    [Min(0.1f)] public float baseLength = 1.2f;  // Длина от центра шара
    [Min(10)] public float baseRotationSpeed = 180f;  // °/сек
    [Min(0.05f)] public float baseWidth = 0.15f; // Ширина хитбокса/спрайта

    [Header("Парирование")]
    public bool canParry = true;
    [Tooltip("Пробивает парирование (Hammer)")]
    public bool ignoresParry = false;
    [Min(0)] public float parryStunDuration = 0.3f;

    [Header("Ranged — только для WeaponBehaviourType.Ranged")]
    [Tooltip("Префаб снаряда")]
    public GameObject projectilePrefab;
    [Tooltip("Интервал стрельбы (сек)")]
    [Min(0.1f)] public float fireInterval = 1.5f;

    [Header("Скейлинг")]
    public WeaponScaling scaling;

    [Header("Префаб визуала")]
    public GameObject weaponPrefab;

    public WeaponRuntimeStats CreateRuntimeStats() => new WeaponRuntimeStats(this);
}
