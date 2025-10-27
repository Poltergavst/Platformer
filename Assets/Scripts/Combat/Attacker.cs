using System;
using UnityEngine;

[RequireComponent (typeof(InputReader), typeof(Weapon))]
public class Attacker : MonoBehaviour
{ 
    [SerializeField] private int _damage;
    [SerializeField] private float _attackRadius;
    [SerializeField] private float _attackCooldown;

    private int hits = 0;
    private int _hitLimit = 1;
    private float _cooldownTimer = 0f;
    private bool _isAttacking = false;

    private Weapon _weapon;

    public event Action<int, int> Attacked;

    private void Awake()
    {
        _weapon = GetComponent<Weapon>();
    }

    private void OnEnable()
    {
        _weapon.CollidedWithDamagable += Attack;
    }

    private void OnDisable()
    {
        _weapon.CollidedWithDamagable -= Attack;
    }

    private void FixedUpdate()
    {
        if (_cooldownTimer > 0f)
        {
            _cooldownTimer -= Time.fixedDeltaTime;
        }
        else
        {
            _isAttacking = false;
        }
    }

    public void InitiateAttack()
    {
        if (_cooldownTimer <= 0f && _isAttacking == false)
        {
            _isAttacking = true;
            _cooldownTimer = _attackCooldown;
            hits = _hitLimit;
            Attacked?.Invoke(PlayerAnimatorStates.PlayerAttack, 1);
        }
    }

    private void Attack(IDamagable enemy)
    {
        if (_isAttacking && hits > 0)
        {
            hits -= 1;
            enemy.TakeDamage(transform.position, _damage);
        }
    }
}
