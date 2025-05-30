using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(PlayerMovement), typeof(Knockbacker))]
[RequireComponent(typeof(PlayerAnimationHandler))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _respawnDelay;
    [SerializeField] private Transform _spawnPoint;

    private Collider2D _collider;

    private PlayerMovement _movement;
    private Knockbacker _knockbacker;
    private PlayerAnimationHandler _animationHandler;

    private bool _isDead;

    private void Awake()
    {
        _isDead = false;

        _collider = GetComponent<Collider2D>();
        _movement = GetComponent<PlayerMovement>();
        _knockbacker = GetComponent<Knockbacker>();
        _animationHandler = GetComponent<PlayerAnimationHandler>();
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    public void TakeHit(Vector3 hitterPosition, bool isLethal)
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

        StartMovement();
    }

    private void Subscribe()
    {
        _knockbacker.KnockbackEnded += StartMovement;
        _knockbacker.KnockbackStarted += StopMovement;

        _movement.Ran += _animationHandler.PlayState;
        _movement.Stopped += _animationHandler.PlayState;
        _movement.Jumped += _animationHandler.PlayState;
        _movement.Fell += _animationHandler.PlayState;
    }

    private void Unsubscribe()
    {
        _knockbacker.KnockbackEnded -= StartMovement;
        _knockbacker.KnockbackStarted -= StopMovement;

        _movement.Ran -= _animationHandler.PlayState;
        _movement.Stopped -= _animationHandler.PlayState;
        _movement.Jumped -= _animationHandler.PlayState;
        _movement.Fell -= _animationHandler.PlayState;
    }
}
