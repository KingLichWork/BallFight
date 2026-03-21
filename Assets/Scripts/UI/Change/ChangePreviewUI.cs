using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangePreviewUI : UIPanel
{
    [SerializeField] private Button _useButton;
    [SerializeField] private Button _closeButton;

    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _description;

    [SerializeField] private Image _image;

    private SelectableItemData _selectedItemData;

    public event Action<SelectableItemData> UseChangeAction;

    private void OnEnable()
    {
        _useButton.onClick.AddListener(Use);
        _closeButton.onClick.AddListener(Hide);
    }

    private void OnDisable()
    {
        _useButton.onClick.RemoveListener(Use);
        _closeButton.onClick.RemoveListener(Hide);
    }

    public void Init(SelectableItemData data, BallData currentData)
    {
        ResetView();

        _selectedItemData = data;

        _name.text = _selectedItemData.itemName;
        //_description.text = ;

        switch (data.changeType)
        {
            case ChangeType.Ball:
                _image.sprite = data.itemSprite;
                _image.color = currentData.ColorData.Color;
                break;
            case ChangeType.Weapon:
                _image.sprite = data.itemSprite;
                break;
            case ChangeType.Color:
                _image.sprite = currentData.ChangeBallData.itemSprite;
                break;
        }
    }

    private void ResetView()
    {
        _image.color = Color.white;
    }

    private void Use()
    {
        Hide();
        UseChangeAction?.Invoke(_selectedItemData);
    }
}
