using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/WeaponsData", fileName = "WeaponsData")]
public class WeaponsData : ScriptableObject
{
    [SerializeField] private List<WeaponData> _weapons = new();

    public List<WeaponData> Weapons => _weapons;
}
