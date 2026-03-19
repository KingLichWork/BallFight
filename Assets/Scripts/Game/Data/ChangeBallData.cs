using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Data/ChangeBallData", fileName = "ChangeBallData")]
public class ChangeBallData : SelectableItemData
{
    public BallShapeType shapeType = BallShapeType.Circle;
    public Sprite ballSprite;

    public Color primaryColor = Color.white;
    public Color accentColor = Color.white;
}

public enum BallShapeType
{
    Circle,
    Square,
    Triangle,
    Star,
    Hexagon,
}