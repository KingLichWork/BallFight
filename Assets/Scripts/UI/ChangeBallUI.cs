using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class ChangeBallUI : UIPanel
{
    [SerializeField] private BallPreviewUI _ballPreview;

    [SerializeField] private ChangeCategory[] _changeCategories;

    [SerializeField] private Button _closeButton;

    private BallData _currentBall;

    [Inject]
    public void Construct()
    {

    }

    private void OnEnable()
    {
        _closeButton.onClick.AddListener(Hide);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(Hide);
    }

    public void Init(BallData ball)
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
