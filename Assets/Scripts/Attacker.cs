using UnityEngine;

[RequireComponent (typeof(InputReader))]
public class Attacker : MonoBehaviour
{ 
    [SerializeField] private int _damage;
    [SerializeField] private float _attackRadius;
    [SerializeField] private Transform _attackAreaOrigin;

    private InputReader _inputReader;
    private Collider2D[] _damagables;

    private void Awake()
    {
        _inputReader = GetComponent<InputReader>();
        _damagables = new Collider2D[5];
    }

    private void FixedUpdate()
    {
        if (_inputReader.IsAttackPressed())
        {
            Debug.Log("Атакую");
            Attack();
        }
    }

    private void Attack()
    {
        Physics2D.OverlapCircleNonAlloc(_attackAreaOrigin.position, _attackRadius, _damagables);

        foreach (Collider2D damagable in _damagables)
        {
            if (damagable == null)
            {
                return;
            }

            if (damagable.gameObject.TryGetComponent<IDamagable>(out IDamagable enemy) && damagable != this.GetComponent<Collider2D>())
            {
                enemy.TakeDamage(transform.position, _damage, false);
            }
        }
    }
}
