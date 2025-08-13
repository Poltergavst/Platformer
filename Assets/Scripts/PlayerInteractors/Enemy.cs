using UnityEngine;


[RequireComponent(typeof(Health), typeof(Knockbacker), typeof(EnemyMovement))]
public class Enemy : PlayerInteractor, IDamagable
{
    [SerializeField] private int _damage;

    private Health _health;
    private Knockbacker _knockbacker;
    private EnemyMovement _movement;

    protected override void Awake()
    {
        base.Awake();

        _health = GetComponent<Health>();
        _knockbacker = GetComponent<Knockbacker>();
        _movement = GetComponent<EnemyMovement>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _knockbacker.KnockbackStarted += _movement.StopMovement;
        _knockbacker.KnockbackEnded += _movement.StartMovement;

        _health.Expired += Die;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _knockbacker.KnockbackStarted -= _movement.StopMovement;
        _knockbacker.KnockbackEnded -= _movement.StartMovement;

        _health.Expired -= Die;
    }

    protected override void InteractWithPlayer(Player player)
    {
        if (IsContactFromAbove(player.transform.position))
        {
            player.GetVerticalBoost();
        }
        else
        {
            player.TakeDamage(transform.position, _damage, false);
        }
    }

    private bool IsContactFromAbove(Vector3 collidedObjectPosition)
    {
        float maxDeviation = 40;
        float deviationFromTop = Vector3.Angle(transform.up, transform.position.DirectionTo(collidedObjectPosition));

        return Mathf.Abs(deviationFromTop) < maxDeviation;
    }

    public void TakeDamage(Vector3 hitterPosition, int damage, bool isLethal)
    {
        _knockbacker.GetKnockbacked(hitterPosition);
        _health.Decrease(damage);
    }

    private void Die()
    {
        _movement.StopMovement();
        gameObject.SetActive(false);

        Invoke(nameof(Respawn), 3);
    }

    private void Respawn()
    {
        gameObject.SetActive(true);

        _health.Reset();
        _movement.StartMovement();
    }
}
