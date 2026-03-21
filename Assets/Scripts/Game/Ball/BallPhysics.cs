using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallPhysics : MonoBehaviour
{
    [Header("Движение")]
    [SerializeField] private float _moveSpeed = 3.5f;
    [SerializeField] private float _minSpeed = 2f;
    [SerializeField] private float _maxSpeed = 7f;

    [Header("Отскок от стен")]
    [SerializeField] private float _wallBounceAngleJitter = 12f;

    [Header("Отталкивание от стен (anti-hug)")]
    [SerializeField] private float _wallRepulsionDistance = 0.6f;
    [SerializeField] private float _wallRepulsionForce = 4f;

    [Header("Отскок при попадании оружием")]
    [SerializeField] private float _hitBounceForce = 5f;

    [SerializeField] private BallShapeType _type;
    [SerializeField] private Transform _weaponParent;

    public Rigidbody2D Rb { get; private set; }

    public Transform WeaponParent => _weaponParent;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        Rb.gravityScale = 0f;
        Rb.linearDamping = 0f;
        Rb.angularDamping = 0f;
        Rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        Collider2D col = GetComponent<Collider2D>();
        col.sharedMaterial = new PhysicsMaterial2D("BallMat")
        {
            bounciness = 1f,
            friction = 0f,
        };
    }

    private void Start() => Launch();

    private void FixedUpdate()
    {
        ClampSpeed();
        ApplyWallRepulsion();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Wall"))
            OnWallBounce();
    }
    public void SetDirection(Vector2 dir)
        => Rb.linearVelocity = dir.normalized * _moveSpeed;

    public void AddImpulse(Vector2 direction, float force)
        => Rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);

    public void BounceFromHit(Vector2 direction)
        => AddImpulse(direction, _hitBounceForce);

    public void Stop()
    {
        Rb.linearVelocity = Vector2.zero;
        Rb.angularVelocity = 0f;
    }

    public void Freeze(bool isAll)
    {
        Rb.constraints = isAll
                ? RigidbodyConstraints2D.FreezeAll
                : RigidbodyConstraints2D.FreezeRotation;
    }

    private void Launch()
    {
        float angle = Random.Range(0f, 360f);

        float mod = angle % 45f;
        if (mod < 10f) angle += 10f;
        else if (mod > 35f) angle -= 10f;

        Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad),
                                    Mathf.Sin(angle * Mathf.Deg2Rad));
        Rb.linearVelocity = dir * _moveSpeed;
    }

    private void OnWallBounce()
    {
        if (Rb.linearVelocity == Vector2.zero) return;

        float jitter = Random.Range(-_wallBounceAngleJitter, _wallBounceAngleJitter);
        float angle = Mathf.Atan2(Rb.linearVelocity.y, Rb.linearVelocity.x)
                        * Mathf.Rad2Deg + jitter;

        float speed = Mathf.Max(Rb.linearVelocity.magnitude, _minSpeed);
        Rb.linearVelocity = new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad)) * speed;
    }

    private void ClampSpeed()
    {
        float sq = Rb.linearVelocity.sqrMagnitude;
        if (sq < _minSpeed * _minSpeed && sq > 0.0001f)
            Rb.linearVelocity = Rb.linearVelocity.normalized * _minSpeed;
        else if (sq > _maxSpeed * _maxSpeed)
            Rb.linearVelocity = Rb.linearVelocity.normalized * _maxSpeed;
    }

    private void ApplyWallRepulsion()
    {
        int wallLayer = LayerMask.GetMask("Default");
        var dirs = new Vector2[]
        {
            Vector2.right, Vector2.left,
            Vector2.up,    Vector2.down,
        };

        foreach (var dir in dirs)
        {
            var hit = Physics2D.Raycast(transform.position, dir,
                                        _wallRepulsionDistance,
                                        LayerMask.GetMask("Default"));
            if (hit.collider != null && hit.collider.CompareTag("Wall"))
            {
                float strength = 1f - hit.distance / _wallRepulsionDistance;
                Rb.AddForce(-dir * _wallRepulsionForce * strength, ForceMode2D.Force);
            }
        }
    }
}
