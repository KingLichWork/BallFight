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

    public event Action<SelectableItemData> UseChangeAction;

    private void OnEnable()
    {
        _selectButton.onClick.AddListener(Use);
    }

    private void OnDisable()
    {
        _selectButton.onClick.RemoveListener(Use);
    }

    public void Init(SelectableItemData data)
    {
        _data = data;
        _image.sprite = _data.itemSprite;
        _name.text = _data.itemName;

       //_lock.SetActive(SaveManager.UnlockedData.);
    }

    private void Use()
    {
        UseChangeAction.Invoke(_data);
    }
}
