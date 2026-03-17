using System;
using UnityEngine;

public class BallHealth : MonoBehaviour
{
    private float _maxHp = 100f;
    private float _invincibilityAfterHit = 0.1f;

    public float MaxHp => _maxHp;
    public float CurrentHp => _currentHp;
    public bool IsAlive => _currentHp > 0f;
    public bool IsInvincible => _invincTimer > 0f;

    public event Action<float, float> OnDamaged; // (damage, currentHp)
    public event Action<BallHealth> OnDied;

    private float _currentHp;
    private float _invincTimer;

    private void Awake()
    {
        _currentHp = _maxHp;
    }

    private void Update()
    {
        if (_invincTimer > 0f)
            _invincTimer -= Time.deltaTime;
    }

    public float TakeDamage(float damage)
    {
        if (!IsAlive || IsInvincible || damage <= 0f) return 0f;

        float actual = Mathf.Min(damage, _currentHp);
        _currentHp -= actual;
        _invincTimer = _invincibilityAfterHit;

        OnDamaged?.Invoke(actual, _currentHp);

        if (_currentHp <= 0f)
        {
            _currentHp = 0f;
            OnDied?.Invoke(this);
        }

        return actual;
    }

    public void ResetHp()
    {
        _currentHp = _maxHp;
        _invincTimer = 0f;
    }

    public void TakeDotDamage(float damage)
    {
        if (!IsAlive || damage <= 0f) return;

        float actual = Mathf.Min(damage, _currentHp);
        _currentHp -= actual;

        OnDamaged?.Invoke(actual, _currentHp);

        if (_currentHp <= 0f)
        {
            _currentHp = 0f;
            OnDied?.Invoke(this);
        }
    }
}
