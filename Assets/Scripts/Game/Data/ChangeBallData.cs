using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Data/ChangeBallData", fileName = "ChangeBallData")]
public class ChangeBallData : SelectableItemData
{
    public BallEntity BallPrefab;
    public Sprite ballSprite;

    public BallShapeType shapeType = BallShapeType.Circle;
}

public enum BallShapeType
{
    Circle,
    Square,
    Triangle,
    Star,
    Hexagon,
}