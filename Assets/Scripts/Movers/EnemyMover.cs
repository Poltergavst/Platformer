using UnityEngine;

[RequireComponent(typeof(GroundChaser), typeof(GroundPatroller), typeof(TargetSearcher))]
public class EnemyMover : MonoBehaviour
{
    TargetSearcher _targetSearcher;

    private GroundMover _movement;
    private GroundChaser _chaseMovement;
    private GroundPatroller _patrolMovement;

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _targetSearcher = GetComponent<TargetSearcher>();

        _chaseMovement = GetComponent<GroundChaser>();
        _patrolMovement = GetComponent<GroundPatroller>();

        _rigidbody = gameObject.GetComponent<Rigidbody2D>();

        _movement = _patrolMovement;
    }

    private void OnEnable()
    {
        _targetSearcher.TargetDetected += SwitchToChase;
        _targetSearcher.TargetLost += SwitchToPatrol; 
    }

    private void OnDisable()
    {
        _targetSearcher.TargetDetected -= SwitchToChase;
        _targetSearcher.TargetLost -= SwitchToPatrol;
    }

    private void FixedUpdate()
    {
        if (_movement.enabled == true)
        {
            _movement.Move();
        }

        if (_movement.IsOverTheEdge(transform.position))
        {
            _rigidbody.velocity = Vector3.zero;
        }
    }

    public void StopMovement()
    {
        _rigidbody.velocity = Vector2.zero;
        _movement.enabled = false;
        _rigidbody.isKinematic = false;
    }

    public void StartMovement()
    {
        _rigidbody.velocity = Vector2.zero;
        _movement.enabled = true;
        _rigidbody.isKinematic = true;
    }

    private void SwitchToPatrol()
    {
        _movement = _patrolMovement;
    }

    private void SwitchToChase()
    {
        _movement = _chaseMovement;
    }
}
