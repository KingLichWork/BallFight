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
    private BallData _currentBall;

    [Inject]
    public void Construct(WeaponsData weaponsData)
    {
        _weaponsData = weaponsData;
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
        }

        //foreach (var weapon in _weaponsData.Weapons)
        //{
        //    ChangeView changeView = Instantiate(_changeBallUIPrefab, _changeCategories[1].ViewParent);
        //    changeView.Init(weapon);
        //}
    }

    public void SetInfo(BallData ball)
    {
        Show();

        _ballPreview.SetInfo(ball);
    }
}

public enum ChangeType
{
    Ball,
    Weapon
}
