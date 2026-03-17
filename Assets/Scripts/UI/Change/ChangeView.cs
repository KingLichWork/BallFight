using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeView : MonoBehaviour
{
    [SerializeField] private Button _selectButton;

    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _name;

    [SerializeField] private GameObject _lock;

    public void Init(WeaponData weapon)
    {
        _image.sprite = weapon.weaponSprite;
        _name.text = weapon.name;

       //_lock.SetActive(SaveManager.UnlockedData.);
    }
}
