using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUI : UIPanel
{
    [SerializeField] private ResourcesViewUI _resourcesViewUI;
    [SerializeField] private ChangeBallUI _changeBallUI;

    [SerializeField] private Button _startButton;

    [SerializeField] private GameObject _changeButtonPrefab;

    private List<Button> _changeButtons = new();
    private Camera _camera;

    public event Action StartBattleAction;

    private void OnEnable()
    {
        _startButton.onClick.AddListener(StartBattle);
    }

    private void OnDisable()
    {
        _startButton.onClick.RemoveListener(StartBattle);
    }

    public void Init()
    {
        _camera = Camera.main;
    }

    public void SpawnChangeButtons(List<BallEntity> balls)
    {
        ClearChangeButtons();

        foreach (BallEntity ball in balls)
        {
            var button = Instantiate(_changeButtonPrefab, transform).GetComponent<Button>();
            button.GetComponent<RectTransform>().position = _camera.WorldToScreenPoint(ball.transform.position - Vector3.up * 3);
            _changeButtons.Add(button.GetComponent<Button>());

            button.onClick.AddListener(_changeBallUI.Show);
        }
    }

    private void StartBattle()
    {
        StartBattleAction.Invoke();
        Hide();
    }

    private void ClearChangeButtons()
    {
        foreach (var button in _changeButtons)
        {
            if (button != null)
            {
                button.onClick.RemoveListener(_changeBallUI.Show);
                Destroy(button.gameObject);
            }
        }

        _changeButtons.Clear();
    }
}
