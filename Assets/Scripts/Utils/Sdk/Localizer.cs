using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace MainAssets.Scripts.Utils
{
    public class Localizer
    {
        public static bool IsRu { get; private set; }

        public static async UniTask Initialize()
        {
            await LocalizationSettings.InitializationOperation;
#if UNITY_EDITOR
            IsRu = true;
#else
            IsRu = YandexGamesSdk.Language.Contains("ru");
#endif
            Debug.Log("IsRu: " + IsRu);
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[IsRu ? 1 : 0];
        }

        public static string GetLocalizedString(TableNames table, string key, params object[] data)
        {
            var localizedString = LocalizationSettings.StringDatabase.GetLocalizedString(table.ToString(), key, data);

            return localizedString;
        }

        public static async UniTask<string> GetLocalizedStringAsync(TableNames table, string key, params object[] data)
        {
            var handle = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(table.ToString(), key, data);
            await handle.Task;

            return handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded
                ? handle.Result
                : key;
        }
    }
}

public enum TableNames
{
    RuEn,
}