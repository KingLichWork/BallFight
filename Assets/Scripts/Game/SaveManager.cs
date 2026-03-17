using Kimicu.YandexGames;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SaveManager
{
    public static PlayerData PlayerData { get; private set; }
    public static PurchasesData PurchasesData { get; private set; }
    public static UnlockedData UnlockedData { get; private set; }

    private static bool _isNeedToSave = false;

    public static IEnumerator LoadAll()
    {
        ResetSaves(() =>
        {
            PurchasesData = Load<PurchasesData>(typeof(PurchasesData).ToString()) ?? new PurchasesData();
            PlayerData = Load<PlayerData>(typeof(PlayerData).ToString()) ?? new PlayerData();
            UnlockedData = Load<UnlockedData>(typeof(UnlockedData).ToString()) ?? new UnlockedData();

            _isNeedToSave = true;
        });

        yield return null;
    }
    public static void DeleteAllSave() => Cloud.ClearCloudData();

    public static T Load<T>(string key)
    {
        var save = Cloud.GetValue<T>(key);
        Debug.Log($"Load key {key}");
        return save;
    }

    public static void Save<T>(T save) where T : class
    {
        if (!_isNeedToSave) return;

        var key = typeof(T).ToString();
        Debug.Log($"Save key {key}");
        Cloud.SetValue(key, save);

        Cloud.SetValue("HasSaves", 1);
        Cloud.SaveInCloud();
    }

    private static void ResetSaves(Action onCallback = null)
    {
        var resetSaves = Cloud.GetValue("ResetSaves", 1) == 1;
        Debug.Log($"Reset saves is {resetSaves}");
        if (!resetSaves)
        {
            onCallback?.Invoke();
            return;
        }

        Cloud.ClearCloudData(onSuccessCallback: () =>
        {
            Debug.Log("Reset Saves complete");
            Cloud.SetValue("ResetSaves", 0);
            onCallback?.Invoke();
        });
    }
}

[Serializable]
public class UnlockedData
{
    public void Save()
    {
        SaveManager.Save(this);
    }
}

[Serializable]
public class PlayerData
{
    private int _coins = 0;
    private int _gems = 0;

    private bool _sounds;
    private bool _music;

    public int Coins { get { return _coins; } set { _coins = value; SaveManager.Save(this); } }
    public int Gems { get { return _gems; } set { _gems = value; SaveManager.Save(this); } }

    public bool Sounds { get { return _sounds; } set { _sounds = value; SaveManager.Save(this); } }
    public bool Music { get { return _music; } set { _music = value; SaveManager.Save(this); } }

    public void Save()
    {
        SaveManager.Save(this);
    }
}

[Serializable]
public class PurchasesData
{
    private bool _noAds = false;

    private int[] _adsCount = new int[2];

    public bool NoAds { get { return _noAds; } set { _noAds = value; SaveManager.Save(this); } }
    public int[] AdsCount { get { return _adsCount; } set { _adsCount = value; SaveManager.Save(this); } }
}