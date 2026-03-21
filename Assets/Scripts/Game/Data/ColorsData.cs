using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/ColorsData", fileName = "ColorsData")]
public class ColorsData : ScriptableObject
{
    [SerializeField] private List<ColorData> _colors = new();

    public List<ColorData> Colors => _colors;
}
