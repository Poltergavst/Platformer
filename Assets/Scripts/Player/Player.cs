using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(PlayerMovement), typeof(Knockbacker))]
[RequireComponent(typeof(PlayerAnimationHandler), typeof(Health))]
public class Player : MonoBehaviour, IDamagable
{
    [SerializeField] private float _respawnDelay;
    [SerializeField] private Transform _spawnPoint;

    private Collider2D _collider;

    private PlayerMovement _movement;
    private Knockbacker _knockbacker;
    private PlayerAnimationHandler _animationHandler;

    private Health _health;

    private bool _isDead;

    private void Awake()
    {
        _isDead = false;

        _collider = GetComponent<Collider2D>();

        _movement = GetComponent<PlayerMovement>();
        _knockbacker = GetComponent<Knockbacker>();
        _animationHandler = GetComponent<PlayerAnimationHandler>();

        _health = GetComponent<Health>(); ;
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    public void TakeDamage(Vector3 hitterPosition, int damage, bool isLethal)
    {
        if (_isDead)
        {
            return;
        }

        _knockbacker.GetKnockbacked(hitterPosition);

        if (isLethal)
        {
            Die();
        }

        _health.Decrease(damage);
    }

    public void GetVerticalBoost()
    {
        _movement.Jump();
    }

    public void StopMovement() 
    {
        _movement.enabled = false;
        _animationHandler.StopAnimations();
    }

    public void StartMovement()
    {
        if (_isDead)
        {
            return;
        }

        _movement.enabled = true;
    }

    private void Die()
    {
        _isDead = true;
        _collider.enabled = false;

        StopMovement();

        StartRespawn();
    }

    private void StartRespawn()
    {
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        WaitForSeconds waitBeforeRespawn = new WaitForSeconds(_respawnDelay);

        yield return waitBeforeRespawn;

        _isDead = false;
        _collider.enabled = true;

        transform.position = _spawnPoint.position;

        _health.Reset();

        StartMovement();
    }

    private void Subscribe()
    {
        _health.Expired += Die;

        _knockbacker.KnockbackEnded += StartMovement;
        _knockbacker.KnockbackStarted += StopMovement;

        _movement.Ran += _animationHandler.PlayState;
        _movement.Stopped += _animationHandler.PlayState;
        _movement.Jumped += _animationHandler.PlayState;
        _movement.Fell += _animationHandler.PlayState;
    }

    private void Unsubscribe()
    {
        _health.Expired -= Die;

        _knockbacker.KnockbackEnded -= StartMovement;
        _knockbacker.KnockbackStarted -= StopMovement;

        _movement.Ran -= _animationHandler.PlayState;
        _movement.Stopped -= _animationHandler.PlayState;
        _movement.Jumped -= _animationHandler.PlayState;
        _movement.Fell -= _animationHandler.PlayState;
    }
}
