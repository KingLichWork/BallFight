using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class BallHealthDisplay : MonoBehaviour
{
    [SerializeField] private string _format = "{0}";
    [SerializeField] private Color _fullColor = Color.white;
    [SerializeField] private Color _lowColor = Color.red;
    [SerializeField] private float _lowThreshold = 0.35f;

    private TextMeshPro _text;
    private BallHealth _health;

    private void Awake()
    {
        _text = GetComponent<TextMeshPro>();
        _health = GetComponentInParent<BallHealth>();

        _health.OnDamaged += OnDamaged;
        _health.OnDied += OnDied;

        Refresh();
    }

    private void OnDestroy()
    {
        if (_health != null)
        {
            _health.OnDamaged -= OnDamaged;
            _health.OnDied -= OnDied;
        }
    }

    private void OnDamaged(float _, float __)
    {
        Refresh();
    }

    private void OnDied(BallHealth _)
    {
        ShowDead();
    }

    private void Refresh()
    {
        int hp = Mathf.CeilToInt(_health.CurrentHp);
        _text.text = string.Format(_format, hp);
        _text.color = _fullColor;
    }

    private void ShowDead()
    {
        _text.text = "0";
        _text.color = _lowColor;
    }
}
