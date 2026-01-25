using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Vampirism : MonoBehaviour
{
    [SerializeField] private float _abilityRadius = 3f;
    [SerializeField] private LayerMask _affectedTargets;

    private float _duration = 6f;
    private float _rechargeTime = 4f;

    private bool _isInProcess = false;

    private Health _health;

    private Coroutine _rechargeCoroutine;
    private Coroutine _abilityCoroutine;

    public event Action<float> ChargeChanged;
    public event Action<float> AbilityStarted;
    public event Action<float> RechargeStarted;
    public event Action RechargeEnded;
    public event Action GotReset;

    public float AbilityRadius => _abilityRadius;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    public void Reset()
    {
        Deactivate();
        GotReset?.Invoke();
    }

    public void Activate()
    {
        if (_isInProcess == false)
        {
            Deactivate();
            _rechargeCoroutine = CoroutineRunner.Instance.StartCoroutine(Recharge());
        }
    }

    public void Deactivate()
    {
        if (_rechargeCoroutine != null)
        {
            CoroutineRunner.Instance.StopCoroutine(_rechargeCoroutine);

            if (_abilityCoroutine != null)
            {
                CoroutineRunner.Instance.StopCoroutine(_abilityCoroutine);
            }
        }

        _isInProcess = false;
    }

    private bool TryGetTarget(out Health target)
    {
        int targetsBufferSize = 5;
        Collider2D closestTarget = null;

        Collider2D[] targetsInRadius = new Collider2D[targetsBufferSize];

        if (Physics2D.OverlapCircleNonAlloc(transform.position, _abilityRadius, targetsInRadius, _affectedTargets) > 0)
        {
            closestTarget = FindClosestTarget(targetsInRadius);
        }

        if (closestTarget == null)
        {
            target = null;
            return false;
        }

        return closestTarget.gameObject.TryGetComponent(out target);
    }

    private Collider2D FindClosestTarget(Collider2D[] targets)
    {
        float shortestDistance = _abilityRadius;
        Collider2D closestTarget = null;

        foreach (Collider2D target in targets)
        {
            if (target == null)
            {
                continue;
            }

            float distance = transform.position.SqrDistanceTo(target.transform.position);

            if (distance <= shortestDistance)
            {
                shortestDistance = distance;
                closestTarget = target;
            }
        }

        return closestTarget;
    }

    private float CalculateHealthPerFrame(float maxHealth)
    {
        float healthAmount;

        float healthPartsCount = 100;
        float healthsPart = 100;

        healthAmount = maxHealth / healthPartsCount * healthsPart;

        return (healthAmount / _duration) * Time.deltaTime;
    }

    private IEnumerator SuckHealth()
    {
        float timeElapsed = 0;

        AbilityStarted?.Invoke(_duration);

        while(timeElapsed < _duration)
        {
            if (TryGetTarget(out Health target))
            {
                target.Decrease(CalculateHealthPerFrame(target.Max));
                _health.Increase(CalculateHealthPerFrame(_health.Max));
            }

            yield return null;

            timeElapsed += Time.deltaTime;

            ChargeChanged?.Invoke(-Time.deltaTime);
        }
    }

    private IEnumerator Recharge()
    {
        float timeElapsed = 0;

        _isInProcess = true;

        _abilityCoroutine = CoroutineRunner.Instance.StartCoroutine(SuckHealth());

        yield return _abilityCoroutine; 
            
        RechargeStarted?.Invoke(_rechargeTime);

        while (timeElapsed < _rechargeTime)
        {
            yield return null;

            timeElapsed += Time.deltaTime;

            ChargeChanged?.Invoke(Time.deltaTime);
        }

        RechargeEnded?.Invoke();
        _isInProcess = false;
    }
}
