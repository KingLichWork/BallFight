using UnityEngine;

[RequireComponent(typeof(BallHealth))]
public class BallEntity : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _ballRenderer;

    [SerializeField] private BallPhysics _ballPhysics;

    public BallHealth Health { get; private set; }
    public BallPhysics BallPhysics => _ballPhysics;
    public WeaponController Weapon { get; private set; }
    public BallData BallData { get; private set; }

    public void Init()
    {
        BallData = new BallData();

        Health = GetComponent<BallHealth>();

        Health.OnDied += OnDied;
    }

    public void AssignBall(ChangeBallData data)
    {
        BallData.SetBall(data);

        GameObject oldBall = _ballPhysics.gameObject;

        InitBall();
        Destroy(oldBall);
    }

    public void AssignColor(ColorData color)
    {
        BallData.SetColor(color);

        _ballRenderer.color = BallData.ColorData.Color;
    }

    public void AssignWeapon(WeaponData data)
    {
        BallData.SetWeapon(data);

        if (Weapon != null)
            Destroy(Weapon.gameObject);

        InitWeapon();
    }

    public void ResetForNewBattle()
    {
        Health.ResetHp();
        Weapon?.ResetStats();
        BallPhysics.SetDirection(Random.insideUnitCircle.normalized);
    }

    private void InitWeapon()
    {
        GameObject weapon = Instantiate(BallData.Weapon.weaponPrefab, _ballPhysics.WeaponParent);

        Weapon = weapon.GetComponent<WeaponController>();

        if (Weapon == null)
            Weapon = weapon.AddComponent<WeaponController>();

        Weapon.Initialize(BallData.Weapon, this);
    }

    private void InitBall()
    {
        GameObject ball = Instantiate(BallData.ChangeBallData.BallPrefab.BallPhysics.gameObject, transform);
        ball.transform.SetSiblingIndex(1);

        _ballPhysics = ball.GetComponent<BallPhysics>();
        _ballRenderer = ball.GetComponent<SpriteRenderer>();

        _ballRenderer.color = BallData.ColorData.Color;

        if(BallData.Weapon != null)
            InitWeapon();

        Health = GetComponentInChildren<BallHealth>();
        _ballPhysics.Freeze(true);
    }

    private void OnDied(BallHealth _)
    {
        BallPhysics.Rb.linearVelocity = Vector2.zero;
        if (Weapon != null) Weapon.enabled = false;

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Health.OnDied -= OnDied;
    }
}
