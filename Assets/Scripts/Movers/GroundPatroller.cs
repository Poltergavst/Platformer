using UnityEngine;

[RequireComponent(typeof(HeightChangeDetector))]
public class GroundPatroller : GroundMover
{
    private Vector2[] _edgeWaypoints;

    protected override void Start()
    {
        base.Start();

        Destination = transform.position;

        _edgeWaypoints = new Vector2[] { RightEdge, LeftEdge};
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

        return Mathf.Abs(Destination.x - transform.position.x) < minDistance;
    }

    private Vector2 GetNextDestination()
    {
        int index = 0;

        return Destination == _edgeWaypoints[index] ? _edgeWaypoints[++index] : _edgeWaypoints[index];
    }
}
