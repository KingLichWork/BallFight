using System;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class GameUI : UIPanel
{
    [SerializeField] private Button _endBattleButton;

    public event Action EndBattleAction;

    [Inject]
    public void Construct()
    {

    }

    private void OnEnable()
    {
        _endBattleButton.onClick.AddListener(EndBattle);
    }

    private void OnDisable()
    {
        _endBattleButton.onClick.RemoveListener(EndBattle);
    }

    private void EndBattle()
    {
        EndBattleAction.Invoke();
    }
}
