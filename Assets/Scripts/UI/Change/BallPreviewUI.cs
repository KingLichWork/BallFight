using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BallPreviewUI : MonoBehaviour
{
    [SerializeField] private Image _ball;
    [SerializeField] private Image _weapon;

    [SerializeField] private TextMeshProUGUI _ballDesc;
    [SerializeField] private TextMeshProUGUI _weaponDesc;

    public void SetInfo(BallData ball)
    {
        _ball.color = ball.ballData.primaryColor;
        _weapon.sprite = ball.Weapon.itemSprite;

        //_ballDesc.text = ;
        //_weaponDesc.text = ;
    }
}
