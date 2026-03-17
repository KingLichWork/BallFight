using Cysharp.Threading.Tasks;
using GameAnalyticsSDK;
using Kimicu.YandexGames;
using Kimicu.YandexGames.Extension;
using MainAssets.Scripts.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using VContainer.Unity;

public class Loading : IInitializable
{
    public async void Initialize()
    {
        await YandexGamesSdk.Initialize();
        await Cloud.Initialize();
        await Billing.Initialize();
        await Localizer.Initialize();
        await Leaderboard.Initialize();

        Advertisement.Initialize();
        WebApplication.Initialize(OnStopGame);
        GameAnalytics.Initialize();

        await Purchase.Initialize();
        await PictureDownloadRoutine();
        await SaveManager.LoadAll();

        await UniTask.WaitForSeconds(1f);

        AdManager.ShowInterstitial(ignoreAdClicker: true);
        SceneManager.LoadScene("Menu");
    }

    private async UniTask PictureDownloadRoutine()
    {
        var currencyData = Billing.CatalogProducts
            .Select(p => (p.id, p.priceCurrencyCode, p.priceCurrencyPicture))
            .ToArray();
        var currencyPictures = new Dictionary<string, (string, Sprite)>();
        foreach ((string id, string code, string pictureUrl) in currencyData)
        {
            if (currencyPictures.ContainsKey(pictureUrl)) continue;

            await PictureExtension.GetPicture(pictureUrl, texture =>
            {
                Rect rect = new(0, 0, texture.width, texture.height);
                Sprite picture = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
                currencyPictures.Add(pictureUrl, (code, picture));
                Currency.CurrencyPictures.Add((id, (code, picture)));
            }, Debug.LogWarning);
            await UniTask.WaitForSeconds(0.5f);
        }
    }

    private void OnStopGame(bool value)
    {
        switch (value)
        {
            case true:
                AudioListener.pause = true;
                Time.timeScale = 0;

                break;

            case (false):

                AudioListener.pause = false;

                Time.timeScale = 1;

                break;
        }
    }
}
public static class Currency
{
    public static List<(string id, (string code, Sprite picture) currency)> CurrencyPictures = new();
}