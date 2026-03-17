using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float lifetime = 4f;

    private BallEntity _owner;
    private WeaponRuntimeStats _stats;
    private WeaponScaling _scaling;
    private Rigidbody2D _rb;
    private float _lifeTimer;
    private bool _hit;

    public void Launch(BallEntity owner, Vector2 direction,
                        WeaponRuntimeStats stats, WeaponScaling scaling)
    {
        _owner = owner;
        _stats = stats;
        _scaling = scaling;
        _rb = GetComponent<Rigidbody2D>();

        _rb.gravityScale = 0f;
        _rb.linearDamping = 0f;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        _rb.linearVelocity = direction.normalized * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        _lifeTimer = lifetime;

        foreach (var ownerCol in owner.GetComponentsInChildren<Collider2D>())
            foreach (var myCol in GetComponents<Collider2D>())
                Physics2D.IgnoreCollision(ownerCol, myCol);
    }

    private void Update()
    {
        _lifeTimer -= Time.deltaTime;
        if (_lifeTimer <= 0f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_hit) return;

        // Уничтожаемся о стену
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
            return;
        }

        // Попадание во вражеский шар
        var enemy = other.GetComponentInParent<BallEntity>();
        if (enemy == null || enemy == _owner) return;

        _hit = true;

        float dealt = enemy.Health.TakeDamage(_stats.damage);
        if (dealt > 0f)
        {
            // Отталкивание по направлению полёта
            enemy.Physics.BounceFromHit(_rb.linearVelocity.normalized);

            // Скейлинг — снаряд тоже вызывает OnHit
            float poison = _scaling != null ? _scaling.OnHit(_stats) : 0f;

            if (poison > 0f)
            {
                var pa = enemy.GetComponent<PoisonApplier>()
                        ?? enemy.gameObject.AddComponent<PoisonApplier>();
                pa.Apply(enemy.Health, poison,
                            _scaling.poisonDamagePerStack,
                            _scaling.poisonTickInterval);
            }
        }

        Destroy(gameObject);
    }
}
