using UnityEngine;

[RequireComponent(typeof(GroundChaser), typeof(GroundPatroller), typeof(TargetSearcher))]
public class EnemyMover : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    private GroundMover _movement;
    private GroundChaser _chaseMovement;
    private GroundPatroller _patrolMovement;

    private TargetSearcher _targetSearcher;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        _chaseMovement = GetComponent<GroundChaser>();
        _patrolMovement = GetComponent<GroundPatroller>();

        _targetSearcher = GetComponent<TargetSearcher>();

        _movement = _patrolMovement;
    }

    private void OnEnable()
    {
        _targetSearcher.TargetLost += SwitchToPatrol; 
        _targetSearcher.TargetDetected += SwitchToChase;
    }

    private void OnDisable()
    {
        _targetSearcher.TargetLost -= SwitchToPatrol;
        _targetSearcher.TargetDetected -= SwitchToChase;
    }

    private void Start()
    {
        float offset = 0.5f;

        _targetSearcher.InitializeSearchArea(_movement.LeftEdge.x - offset, _movement.RightEdge.x + offset, _movement.LeftEdge.y - offset);
    }

    private void FixedUpdate()
    {
        _targetSearcher.SearchForTarget(_movement.GetDirection());

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
        _chaseMovement.SetOnTarget(_targetSearcher.Target.position);

        _movement = _chaseMovement;
    }
}
