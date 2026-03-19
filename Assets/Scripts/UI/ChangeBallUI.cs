using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class ChangeBallUI : UIPanel
{
    [SerializeField] private BallPreviewUI _ballPreview;

    [SerializeField] private ChangeCategory[] _changeCategories;
    [SerializeField] private ChangeView _changeBallUIPrefab;

    [SerializeField] private Button _closeButton;

    private WeaponsData _weaponsData;
    private ChangesBallData _changesBallData;

    private BallData _currentBall;

    [Inject]
    public void Construct(WeaponsData weaponsData, ChangesBallData changesBallData)
    {
        _weaponsData = weaponsData;
        _changesBallData = changesBallData;

        Init();
    }

    private void OnEnable()
    {
        _closeButton.onClick.AddListener(Hide);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(Hide);
    }

    public void Init()
    {
        foreach (var weapon in _weaponsData.Weapons)
        {
            ChangeView changeView = Instantiate(_changeBallUIPrefab, _changeCategories[1].ViewParent);
            changeView.Init(weapon);

            changeView.UseChangeAction += UseChange;
        }

        foreach (var changeBall in _weaponsData.Weapons)
        {
            ChangeView changeView = Instantiate(_changeBallUIPrefab, _changeCategories[0].ViewParent);
            changeView.Init(changeBall);

            changeView.UseChangeAction += UseChange;
        }
    }

    public void SetInfo(BallData ball)
    {
        _currentBall = ball;
        _ballPreview.SetInfo(_currentBall);

        Show();
    }

    private void UseChange(ChangeType type)
    {

    }
}

public enum ChangeType
{
    Ball,
    Weapon
}
