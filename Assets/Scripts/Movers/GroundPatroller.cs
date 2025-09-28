using UnityEngine;

[RequireComponent(typeof(HeightChangeDetector))]
public class GroundPatroller : GroundMover
{
    private Vector2 _leftWaypoint, _rightWaypoint;

    protected override void Start()
    {
        float offset = 0.1f;

        base.Start();

        Destination = transform.position;

        _leftWaypoint = RightEdge + Vector2.left * offset;
        _rightWaypoint = LeftEdge + Vector2.right * offset;
    }

    public override void Move() => Patrol();

    private void Patrol()
    {
        Rotator.Turn(Rigidbody.position.x, Destination.x);

        if (HasReachedDestination())
        {
            Destination = GetNextDestination();
        }

        base.Move();
    }

    private bool HasReachedDestination()
    {
        float minDistance = 0.2f;

        return Mathf.Abs(Destination.x - transform.position.x) < minDistance; //Rigidbody.position.IsEnoughCloseTo(Destination, 0.3f);
    }

    private Vector2 GetNextDestination()
    {
        return Destination == _leftWaypoint ? _rightWaypoint : _leftWaypoint;
    }
}
