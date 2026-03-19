using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/ChangesBallData", fileName = "ChangesBallData")]
public class ChangesBallData : ScriptableObject
{
    [SerializeField] private List<ChangeBallData> _balls = new();

    public List<ChangeBallData> Balls => _balls;
}
