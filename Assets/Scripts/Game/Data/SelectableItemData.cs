using UnityEngine;

public abstract class SelectableItemData : ScriptableObject
{
    [Header("Общие параметры")]
    public string itemName = "Item";
    public Sprite itemSprite;
    public bool isUnlocked = true;

    public ChangeType changeType;
}
