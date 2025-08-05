using UnityEngine;

[RequireComponent(typeof(GroundChaser), typeof(GroundPatroller))]
public class EnemyMovement : MonoBehaviour
{
    private GroundMover _movement;
    private GroundChaser _chaseMovement;
    private GroundPatroller _patrolMovement;

    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;

    private Color _baseColor;

    private void OnEnable()
    {
        _chaseMovement.TargetDetected += SwitchToChase;
        _chaseMovement.TargetLost += SwitchToPatrol; 
    }

    private void OnDisable()
    {
        _chaseMovement.TargetDetected -= SwitchToChase;
        _chaseMovement.TargetDetected -= SwitchToPatrol;
    }

    private void Awake()
    {
        _chaseMovement = GetComponent<GroundChaser>();
        _patrolMovement = GetComponent<GroundPatroller>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _rb = gameObject.GetComponent<Rigidbody2D>();

        _baseColor = _spriteRenderer.color;

        _movement = _patrolMovement;
    }

    private void FixedUpdate()
    {
        if (_movement.enabled == true)
        {
            _movement.Move();
        }

        if (_movement.IsOverTheEdge(transform.position))
        {
            _rb.velocity = Vector3.zero;
        }
    }

    public void StopMovement()
    {
        _rb.velocity = Vector2.zero;
        _movement.enabled = false;
        _rb.isKinematic = false;
    }

    public void StartMovement()
    {
        _rb.velocity = Vector2.zero;
        _movement.enabled = true;
        _rb.isKinematic = true;
    }

    private void SwitchToPatrol()
    {
        _spriteRenderer.color = _baseColor;
        _movement = _patrolMovement;
    }

    private void SwitchToChase()
    {
        _spriteRenderer.color = Color.red;
        _movement = _chaseMovement;
    }
}
