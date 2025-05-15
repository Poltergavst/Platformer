using UnityEngine;

public class OneHeightPatroller : Mover
{
    private HeightChangeDetector _heightChangeDetector;

    private Vector2 _leftWaypoint, _rightWaypoint;
    private Vector2 _currentDestination;

    protected override void Awake()
    {
        base.Awake();

        _heightChangeDetector = GetComponent<HeightChangeDetector>();
    }

    private void Start()
    {
        _currentDestination = transform.position;

        _leftWaypoint = _heightChangeDetector.FindEdgeInDirection(Vector2.left, _groundChecker);
        _rightWaypoint = _heightChangeDetector.FindEdgeInDirection(Vector2.right, _groundChecker);
    }

    protected override void FixedUpdate()
    {
        if (HasReachedDestination())
        {
            _currentDestination = GetNextDestination();
            Turn(_rigidbody.position.x, _currentDestination.x);
        }

        base.FixedUpdate();
    }

    protected override void Move()
    {
        Vector2 newPosition = _rigidbody.position + _rigidbody.position.DirectionTo(_currentDestination) * _speed * Time.fixedDeltaTime;

        _rigidbody.MovePosition(newPosition);
    }

    private bool HasReachedDestination()
    {
        return _rigidbody.position.IsEnoughCloseTo(_currentDestination, 0.3f);
    }

    private Vector2 GetNextDestination()
    {
        return _currentDestination == _leftWaypoint ? _rightWaypoint : _leftWaypoint;
    }
}
