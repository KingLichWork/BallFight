using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private GraphicRaycaster _graphicRaycaster;
    [SerializeField] private StartUI _startUI;
    [SerializeField] private GameUI _gameUI;

    [SerializeField] private WeaponsData _weaponData;
    [SerializeField] private ArenaController _arenaController;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(_startUI);
        builder.RegisterComponent(_gameUI);

        builder.RegisterComponent(_weaponData);
        builder.RegisterComponent(_arenaController);

        builder.RegisterEntryPoint<GameBootstrap>();
    }
}
