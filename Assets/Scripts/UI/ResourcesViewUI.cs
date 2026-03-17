using TMPro;
using UnityEngine;
using UI.Money;

public class ResourcesViewUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _resources;

    private void OnEnable()
    {
        ResourcesWallet.OnResourcesCountChanged += ChangeResourcesValue;
    }

    private void OnDisable()
    {
        ResourcesWallet.OnResourcesCountChanged -= ChangeResourcesValue;
    }

    private void ChangeResourcesValue(ResourcesType type, int value)
    {
        _resources[(int)type].text = value.ToString();
    }
}
