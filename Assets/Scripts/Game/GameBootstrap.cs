using FindTheDifference.Audio;
using Kimicu.YandexGames;
using System;
using UnityEngine;
using UnityEngine.InputSystem.XInput;
using VContainer;
using VContainer.Unity;

public class GameBootstrap : IInitializable
{
    private ArenaController _arenaController;
    private StartUI _startUI;

    [Inject]
    public GameBootstrap(ArenaController arenaController, StartUI startUI)
    {
        _arenaController = arenaController;
        _startUI = startUI;
    }

    public void Initialize()
    {
        try
        {
            SaveManager.LoadAll();

            _startUI.Init();
            _arenaController.Init();

            //YandexGamesSdk.GameReady();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}
