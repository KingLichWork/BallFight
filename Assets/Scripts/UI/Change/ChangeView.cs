using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.LightingExplorerTableColumn;

public class ChangeView : MonoBehaviour
{
    [SerializeField] private Button _selectButton;

    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _name;

    [SerializeField] private GameObject _lock;

    private SelectableItemData _data;
    private BallData _currentData;

    public event Action<SelectableItemData> UseChangeAction;

    private void OnEnable()
    {
        _selectButton.onClick.AddListener(Use);
    }

    private void OnDisable()
    {
        _selectButton.onClick.RemoveListener(Use);
    }

    public void Init(SelectableItemData data, BallData currentData)
    {
        _data = data;
        _image.sprite = _data.itemSprite;
        _name.text = _data.itemName;
        _currentData = currentData;

        SetView(data, _currentData);

        //_lock.SetActive(SaveManager.UnlockedData.);
    }

    public void SetView(SelectableItemData data, BallData currentData)
    {
        _currentData = currentData;

        switch (data.changeType)
        {
            case ChangeType.Ball:
                _image.sprite = data.itemSprite;
                _image.color = _currentData.ColorData.Color;
                break;
            case ChangeType.Weapon:
                _image.sprite = data.itemSprite;
                break;
            case ChangeType.Color:
                _image.sprite = currentData.ChangeBallData.itemSprite;
                break;
        }
    }

    private void Use()
    {
        UseChangeAction.Invoke(_data);
    }
}
