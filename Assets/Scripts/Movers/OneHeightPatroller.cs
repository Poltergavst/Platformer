using UnityEngine;

[RequireComponent(typeof(HeightChangeDetector))]
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

        _leftWaypoint = _heightChangeDetector.FindEdgeInDirection(Vector2.left, GroundChecker);
        _rightWaypoint = _heightChangeDetector.FindEdgeInDirection(Vector2.right, GroundChecker);
    }

    protected override void FixedUpdate()
    {
        if (HasReachedDestination())
        {
            _currentDestination = GetNextDestination();
            Rotator.Turn(Rigidbody.position.x, _currentDestination.x);
        }

        base.FixedUpdate();
    }

    protected override void Move()
    {
        Vector2 newPosition = Rigidbody.position + Rigidbody.position.DirectionTo(_currentDestination) * Speed * Time.fixedDeltaTime;

        Rigidbody.MovePosition(newPosition);
    }

    private bool HasReachedDestination()
    {
        return Rigidbody.position.IsEnoughCloseTo(_currentDestination, 0.3f);
    }

    private Vector2 GetNextDestination()
    {
        return _currentDestination == _leftWaypoint ? _rightWaypoint : _leftWaypoint;
    }
}
