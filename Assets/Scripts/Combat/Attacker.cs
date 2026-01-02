using System;
using Unity.VisualScripting;
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
    private float _weaponBasicDistance;
    private float _weaponLastDistance;

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

    private void Start()
    {
        _weaponBasicDistance = Mathf.Abs(_weapon.GetPosition().x - transform.position.x);
    }

    private void FixedUpdate()
    {
        float weaponDistance;

        weaponDistance = Mathf.Abs(_weapon.GetPosition().x - transform.position.x);

        if (_cooldownTimer > 0f)
        {
            _cooldownTimer -= Time.fixedDeltaTime;

            if(IsAttackEnding(weaponDistance))
            {
                _isAttacking = false;
            }
        }
        else
        {
            _isAttacking = false;
        }

        _weaponLastDistance = Mathf.Abs(_weapon.GetPosition().x - transform.position.x);
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

    private bool IsAttackEnding(float weaponDistance)
    {
        if (_weaponLastDistance > _weaponBasicDistance)
        {
            if (weaponDistance - _weaponBasicDistance < 0.2)
            {
                if (_weaponLastDistance > weaponDistance)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
