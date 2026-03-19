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

    public void Init(SelectableItemData data)
    {
        _selectedItemData = data;

        _image.sprite = _selectedItemData.itemSprite;
        _name.text = _selectedItemData.itemName;

        //_description.text = ;
    }

    private void Use()
    {
        Hide();
        UseChangeAction.Invoke(_selectedItemData);
    }
}
