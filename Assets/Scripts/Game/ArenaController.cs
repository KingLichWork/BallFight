using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class ArenaController : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnPoints;

    private int _spawnCount = 2;

    private List<BallEntity> _balls = new();

    private WeaponsData _weaponsData;
    private ChangesBallData _changesBallData;

    private StartUI _startUI;
    private GameUI _gameUI;
    private IObjectResolver _objectResolver;

    private bool _battleRunning;

    [Inject]
    public void Construct(StartUI startUI, GameUI gameUI, WeaponsData weaponsData, ChangesBallData changesBallData, IObjectResolver objectResolver)
    {
        _startUI = startUI;
        _gameUI = gameUI;
        _weaponsData = weaponsData;
        _changesBallData = changesBallData;
        _objectResolver = objectResolver;
    }

    private void OnEnable()
    {
        _startUI.StartBattleAction += StartBattle;
        _gameUI.EndBattleAction += EndBattle;
    }

    private void OnDisable()
    {
        _startUI.StartBattleAction -= StartBattle;
        _gameUI.EndBattleAction -= EndBattle;
    }

    public void Init()
    {
        SpawnBalls();
    }

    private void StartBattle()
    {
        RunBattle();
        _gameUI.Show();
    }

    private void EndBattle()
    {
        _gameUI.Hide();
        _startUI.Show();
        SpawnBalls();
    }

    private void RunBattle()
    {
        FreezeAll(false);
        _battleRunning = true;
    }

    private void SpawnBalls()
    {
        ClearBalls();

        for (int i = 0; i < _spawnCount; i++)
        {
            var ball = Instantiate(_changesBallData.Balls[0].BallPrefab, _spawnPoints[i].position, Quaternion.identity, _spawnPoints[i]).GetComponent<BallEntity>();

            ball.Init();
            ball.AssignBall(_changesBallData.Balls[0]);
            ball.AssignWeapon(_weaponsData.Weapons[0]);
            //ball.PlayerIndex = i;
            ball.Health.OnDied += OnBallDied;

            _balls.Add(ball);
        }

        FreezeAll(true);

        _startUI.SpawnChangeButtons(_balls);
    }

    private void ClearBalls()
    {
        foreach (var b in _balls)
            if (b != null)
                Destroy(b.gameObject);

        _balls.Clear();
    }

    private void FreezeAll(bool freeze)
    {
        foreach (var b in _balls)
        {
            if (b == null) 
                continue;

            var rb = b.BallPhysics.Rb;
            rb.constraints = freeze
                ? RigidbodyConstraints2D.FreezeAll
                : RigidbodyConstraints2D.FreezeRotation;

            if (!freeze)
            {
                b.BallPhysics.SetDirection(Random.insideUnitCircle.normalized);
                b.Weapon.StartFight();
            }
        }
    }

    private void OnBallDied(BallHealth _)
    {
        if (!_battleRunning) return;

        int alive = 0;
        BallEntity lastAlive = null;
        foreach (var b in _balls)
            if (b != null && b.Health.IsAlive) { alive++; lastAlive = b; }

        if (alive <= 1)
        {
            _battleRunning = false;
            string winner = lastAlive != null ? lastAlive.BallData.Weapon.itemName : "Ничья";
            Debug.Log($"[Arena] Победитель: {winner}!");
            EndBattle();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0.5f, 0.15f);
        Gizmos.DrawCube(transform.position, new Vector3(18f, 10f, 0f));
        Gizmos.color = new Color(0f, 1f, 0.5f, 0.8f);
        Gizmos.DrawWireCube(transform.position, new Vector3(18f, 10f, 0f));
    }
#endif
}
