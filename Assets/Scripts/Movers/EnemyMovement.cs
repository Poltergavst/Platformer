using UnityEngine;

[RequireComponent(typeof(GroundChaser), typeof(GroundPatroller))]
public class EnemyMovement : MonoBehaviour
{
    GroundMover _movement;
    GroundChaser _chaseMovement;
    GroundPatroller _patrolMovement;

    SpriteRenderer _spriteRenderer;
    Color _baseColor;

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

        _baseColor = _spriteRenderer.color;

        _movement = _patrolMovement;
    }

    private void FixedUpdate()
    {
        _movement.Move();
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
