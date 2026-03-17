using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private WeaponData _data;
    private BallEntity _owner;
    private WeaponRuntimeStats _stats;

    private float _currentAngle;
    private bool _isParryStunned;
    private float _parryStunTimer;
    private float _fireTimer;        // Ranged: время до следующего выстрела
    private bool _isСlockwise;

    private SpriteRenderer _sr;
    private Collider2D _col;

    public WeaponRuntimeStats Stats => _stats;
    public WeaponData Data => _data;
    public BallEntity Owner => _owner;
    public bool IsStunned => _isParryStunned;

    public void Initialize(WeaponData data, BallEntity owner)
    {
        _data = data;
        _owner = owner;
        _stats = data.CreateRuntimeStats();

        _sr = GetComponentInChildren<SpriteRenderer>();
        _col = GetComponent<Collider2D>() ?? gameObject.AddComponent<BoxCollider2D>();
        _col.isTrigger = true;

        _fireTimer = data.fireInterval;

        UpdateTransform();
    }

    public void ResetStats()
    {
        _stats = _data.CreateRuntimeStats();
        _isParryStunned = false;
        _parryStunTimer = 0f;
        _fireTimer = _data.fireInterval;
        UpdateTransform();
    }

    private void Update()
    {
        if (_isParryStunned)
        {
            _parryStunTimer -= Time.deltaTime;
            if (_parryStunTimer <= 0f) _isParryStunned = false;
            return;
        }

        _currentAngle = (_currentAngle + _stats.rotationSpeed * Time.deltaTime * (_isСlockwise ? 1 : -1)) % 360f;
        UpdateTransform();

        if (_data.behaviourType == WeaponType.Ranged)
            TickRangedFire();
    }

    private void TickRangedFire()
    {
        _fireTimer -= Time.deltaTime;
        if (_fireTimer > 0f) return;

        _fireTimer = _stats.fireInterval;

        if (_data.projectilePrefab == null) return;

        Vector2 fireDir = transform.up;

        Vector3 spawnPos = transform.position + (Vector3)(fireDir * (_stats.length * 0.5f));
        var go = Instantiate(_data.projectilePrefab, spawnPos, Quaternion.identity);
        var proj = go.GetComponent<Projectile>();
        proj?.Launch(_owner, fireDir, _stats, _data.scaling);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isParryStunned) return;

        // Касание чужого оружия парирование
        var enemyWeapon = other.GetComponent<WeaponController>();
        if (enemyWeapon != null && enemyWeapon.Owner != _owner)
        {
            HandleWeaponCollision(enemyWeapon);
            return;
        }

        // Касание тела вражеского шара

        var enemyBall = other.GetComponentInParent<BallEntity>();
        if (enemyBall != null && enemyBall != _owner)
            HandleHitOnBall(enemyBall);

        _isСlockwise = !_isСlockwise;
    }

    // ── Попадание по шару ─────────────────────────────────────────────

    private void HandleHitOnBall(BallEntity target)
    {
        float dealt = target.Health.TakeDamage(_stats.damage);
        if (dealt <= 0f) return;

        // Отталкивание: от атакующего к цели
        Vector2 dir = (target.transform.position - _owner.transform.position).normalized;
        target.Physics.BounceFromHit(dir);

        // Небольшой обратный отскок атакующего — ощущение удара
        _owner.Physics.AddImpulse(-dir, 1.5f);

        ApplyScalingOnHit(target);
        UpdateTransform();
        //PlaySound(_data.hitSound);
    }

    // ── Парирование ───────────────────────────────────────────────────

    private void HandleWeaponCollision(WeaponController other)
    {
        bool thisIsShield = _data.behaviourType == WeaponType.Shield;
        bool otherIsShield = other._data.behaviourType == WeaponType.Shield;

        // Hammer / ignoresParry — пробивает блок
        if (other._data.ignoresParry && !thisIsShield)
        {
            // Атакующий пробивает — цель оглушается, атакующий нет
            ApplyParryStun(_data.parryStunDuration);
            BounceApart(other);
            return;
        }

        if (!_data.canParry) return;

        // Щит блокирует всегда дольше
        float stunSelf = thisIsShield ? _data.parryStunDuration * 0.5f : _data.parryStunDuration;
        float stunOther = otherIsShield ? other._data.parryStunDuration * 0.5f : other._data.parryStunDuration;

        ApplyParryStun(stunSelf);
        other.ApplyParryStun(stunOther);

        BounceApart(other);

        // Скейлинг парирования
        _data.scaling?.OnParry(_stats);
        other._data.scaling?.OnParry(other._stats);

        // Обновить трансформы — мог измениться width (щит)
        UpdateTransform();
        other.UpdateTransformPublic();

        //PlaySound(_data.parrySound);
    }

    private void BounceApart(WeaponController other)
    {
        // Направление — от центра другого шара к нашему
        Vector2 dir = (_owner.transform.position - other._owner.transform.position).normalized;
        if (dir == Vector2.zero) dir = Random.insideUnitCircle.normalized;

        _owner.Physics.AddImpulse(dir, 5f);
        other._owner.Physics.AddImpulse(-dir, 5f);
    }

    public void ApplyParryStun(float duration)
    {
        _isParryStunned = true;
        _parryStunTimer = Mathf.Max(_parryStunTimer, duration);
    }


    private void ApplyScalingOnHit(BallEntity target)
    {
        if (_data.scaling == null) return;

        float poison = _data.scaling.OnHit(_stats);

        if (poison > 0f)
        {
            var pa = target.GetComponent<PoisonApplier>()
                  ?? target.gameObject.AddComponent<PoisonApplier>();
            pa.Apply(target.Health, poison,
                     _data.scaling.poisonDamagePerStack,
                     _data.scaling.poisonTickInterval);
        }
    }

    private void UpdateTransform()
    {
        float rad = _currentAngle * Mathf.Deg2Rad;
        var outDir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

        transform.localPosition = outDir * (_stats.length * 0.5f);
        transform.localRotation = Quaternion.Euler(0f, 0f, _currentAngle - 90f);

        // Коллайдер вдоль Y (длина оружия)
        if (_col is BoxCollider2D box)
            box.size = new Vector2(_stats.width, _stats.length);

        // Спрайт — X = ширина, Y = длина
        if (_sr != null)
            _sr.size = new Vector2(_stats.width, _stats.length);
    }

    public void UpdateTransformPublic() => UpdateTransform();

    private void PlaySound(AudioClip clip)
    {
        if (clip != null) AudioSource.PlayClipAtPoint(clip, transform.position, 0.8f);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (_stats == null || _owner == null) return;

        float rad = _currentAngle * Mathf.Deg2Rad;
        Vector2 outDir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        Vector3 root = _owner.transform.position;
        Vector3 tip = root + (Vector3)(outDir * _stats.length);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(root, 0.06f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(root, tip);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(tip, 0.06f);
    }
#endif
}
