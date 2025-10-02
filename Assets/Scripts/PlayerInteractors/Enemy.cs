using UnityEngine;

[RequireComponent(typeof(Health), typeof(Knockbacker), typeof(EnemyMovement))]
public class Enemy : PlayerInteractor, IDamagable
{
    [SerializeField] private int _damage;

    private Health _health;
    private Knockbacker _knockbacker;
    private EnemyMovement _movement;

    private Vector3 _defaultPosition, _defaultScale;

    protected override void Awake()
    {
        base.Awake();

        _health = GetComponent<Health>();
        _knockbacker = GetComponent<Knockbacker>();
        _movement = GetComponent<EnemyMovement>();

        _defaultScale = transform.localScale;
        _defaultPosition = transform.position;
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
            GetStomped();

            player.GetVerticalBoost();
        }
        else
        {
            player.TakeDamage(transform.position, _damage);
        }
    }

    private bool IsContactFromAbove(Vector3 collidedObjectPosition)
    {
        float maxDeviation = 40;
        float deviationFromTop = Vector3.Angle(transform.up, transform.position.DirectionTo(collidedObjectPosition));

        return Mathf.Abs(deviationFromTop) < maxDeviation;
    }

    public void TakeDamage(Vector3 hitterPosition, int damage)
    {
        _knockbacker.TakeKnockback(hitterPosition);
        _health.Decrease(damage);
    }

    public void GetStomped()
    {
        float half = 0.5f;
        float lessenVerticalScale = 0.3f * transform.localScale.y;
        
        int damageFromStomp = 1;

        transform.localScale = transform.localScale.Change(y: transform.localScale.y - lessenVerticalScale);
        transform.position = transform.position.Change(y: transform.position.y - lessenVerticalScale * half);

        _health.Decrease(damageFromStomp);
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

        transform.position = new Vector3(transform.position.x, _defaultPosition.y, transform.position.z);
        transform.localScale = _defaultScale;

        _health.Reset();
        _movement.StartMovement();
    }
}
