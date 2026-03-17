using UnityEngine;

[RequireComponent(typeof(BallHealth))]
[RequireComponent(typeof(BallPhysics))]
public class BallEntity : MonoBehaviour
{
    [Header("Визуал")]
    [SerializeField] private SpriteRenderer _ballRenderer;
    [SerializeField] private Transform _weaponRoot;

    public BallHealth Health { get; private set; }
    public BallPhysics Physics { get; private set; }
    public WeaponController Weapon { get; private set; }
    public BallData BallData { get; private set; }

    public void Init()
    {
        BallData = new BallData();

        Health = GetComponent<BallHealth>();
        Physics = GetComponent<BallPhysics>();

        Health.OnDied += OnDied;
    }

    public void AssignWeapon(WeaponData data)
    {
        BallData.SetWeapon(data);

        if (Weapon != null)
            Destroy(Weapon.gameObject);

        InitWeapon();
        ApplyColors();
    }

    public void ResetForNewBattle()
    {
        Health.ResetHp();
        Weapon?.ResetStats();
        Physics.SetDirection(Random.insideUnitCircle.normalized);
    }

    private void InitWeapon()
    {
        Transform parent = _weaponRoot != null ? _weaponRoot : transform;

        GameObject weapon = Instantiate(BallData.Weapon.weaponPrefab, parent);

        Weapon = weapon.GetComponent<WeaponController>();

        if (Weapon == null)
            Weapon = weapon.AddComponent<WeaponController>();

        Weapon.Initialize(BallData.Weapon, this);
    }

    private void ApplyColors()
    {
        //if (_ballRenderer != null && _weaponData != null)
        //    _ballRenderer.color = _weaponData.ballColor;
    }

    private void OnDied(BallHealth _)
    {
        Physics.Rb.linearVelocity = Vector2.zero;
        if (Weapon != null) Weapon.enabled = false;

        Destroy(gameObject);
    }
}
