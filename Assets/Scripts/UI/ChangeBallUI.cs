using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using VContainer;
using Button = UnityEngine.UI.Button;

public class ChangeBallUI : UIPanel
{
    [SerializeField] private BallPreviewUI _ballPreview;

    [SerializeField] private ChangeCategory[] _changeCategories;
    [SerializeField] private ChangeView _changeBallUIPrefab;

    [SerializeField] private Button _closeButton;
    [SerializeField] private RectTransform _contentScrollView;

    private WeaponsData _weaponsData;
    private ChangesBallData _changesBallData;
    private ChangePreviewUI _changePreviewUI;

    private BallData _currentBallData;
    private BallEntity _currentBall;

    [Inject]
    public void Construct(WeaponsData weaponsData, ChangesBallData changesBallData, ChangePreviewUI changePreviewUI)
    {
        _weaponsData = weaponsData;
        _changesBallData = changesBallData;
        _changePreviewUI = changePreviewUI;

        Init();
    }

    private void OnEnable()
    {
        _changePreviewUI.UseChangeAction += UseChange;

        _closeButton.onClick.AddListener(Hide);
    }

    private void OnDisable()
    {
        _changePreviewUI.UseChangeAction -= UseChange;

        _closeButton.onClick.RemoveListener(Hide);
    }

    public void Init()
    {
        foreach (var weapon in _weaponsData.Weapons)
        {
            ChangeView changeView = Instantiate(_changeBallUIPrefab, _changeCategories[1].ViewParent);
            changeView.Init(weapon);

            changeView.UseChangeAction += ShowChange;
        }


        foreach (var changeBall in _changesBallData.Balls)
        {
            ChangeView changeView = Instantiate(_changeBallUIPrefab, _changeCategories[0].ViewParent);
            changeView.Init(changeBall);

            changeView.UseChangeAction += ShowChange;
        }

        foreach(var category in _changeCategories)
        {
            category.Init();
        }

        float height = _changeCategories[0].SizeY + _changeCategories[1].SizeY + 20;
        _contentScrollView.sizeDelta = new Vector2(_contentScrollView.sizeDelta.x, height);
    }

    public void Init(BallEntity ball, BallData ballData)
    {
        _currentBall = ball;
        SetInfo(ballData);
        Show();
    }

    private void SetInfo(BallData ball)
    {
        _currentBallData = ball;
        _ballPreview.SetInfo(_currentBallData);
    }

    private void ShowChange(SelectableItemData data)
    {
        _changePreviewUI.Init(data);
        _changePreviewUI.Show();
    }

    private void UseChange(SelectableItemData data)
    {
        if(data.changeType == ChangeType.Ball)
            _currentBall.AssignBall((ChangeBallData)data);
        else
            _currentBall.AssignWeapon((WeaponData)data);

        _ballPreview.SetInfo(_currentBallData);
    }

    protected override void OnHide()
    {
        base.OnHide();
        _changePreviewUI.Hide();
    }
}

public enum ChangeType
{
    Ball,
    Weapon
}
