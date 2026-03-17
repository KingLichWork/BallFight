using System.Collections;
using UnityEngine;

public class PoisonApplier : MonoBehaviour
{
    private BallHealth _target;
    private float _damagePerStack;
    private float _tickInterval;
    private Coroutine _tickCoroutine;

    private float _stacks;

    public void Apply(BallHealth target, float newStacks, float damagePerStack, float tickInterval)
    {
        _target = target;
        _damagePerStack = damagePerStack;
        _tickInterval = tickInterval;
        _stacks += newStacks;

        if (_tickCoroutine == null)
            _tickCoroutine = StartCoroutine(TickLoop());
    }

    private IEnumerator TickLoop()
    {
        while (_stacks > 0f && _target != null && _target.IsAlive)
        {
            yield return new WaitForSeconds(_tickInterval);

            float consumed = Mathf.Min(1f, _stacks);
            _stacks -= consumed;
            _target.TakeDotDamage(_damagePerStack * consumed);
        }

        _tickCoroutine = null;
        Destroy(this);
    }
}
