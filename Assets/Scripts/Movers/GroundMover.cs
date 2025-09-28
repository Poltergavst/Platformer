using UnityEngine;

[RequireComponent(typeof(GroundChecker), typeof(HeightChangeDetector))]
public abstract class GroundMover: Mover
{
    protected Vector2 Destination;
    protected GroundChecker GroundChecker;
    protected HeightChangeDetector HeightChangeDetector;

    protected Vector2 LeftEdge;
    protected Vector2 RightEdge;

    protected override void Awake()
    {
        base.Awake();

        GroundChecker = GetComponent<GroundChecker>();
        HeightChangeDetector = GetComponent<HeightChangeDetector>();
    }

    protected virtual void Start()
    {
        LeftEdge = HeightChangeDetector.FindEdgeInDirection(Vector2.left, GroundChecker);
        RightEdge = HeightChangeDetector.FindEdgeInDirection(Vector2.right, GroundChecker);
    }

    public override void Move()
    {
        Vector2 newPosition = Rigidbody.position + Rigidbody.position.DirectionTo(Destination) * Speed * Time.fixedDeltaTime;

        if (IsOverTheEdge(newPosition) == true)
        {
            newPosition = SetToNearestEdge(newPosition);
        }

        Rigidbody.MovePosition(newPosition);
    }

    public bool IsOverTheEdge(Vector2 position)
    {
        return position.x > RightEdge.x || position.x < LeftEdge.x;
    }

    public Vector2 SetToNearestEdge(Vector2 position)
    {
        if (position.SqrDistanceTo(RightEdge) < position.SqrDistanceTo(LeftEdge))
        {
            position = RightEdge;
        }
        else
        {
            position = LeftEdge;
        }

        return position;
    }
}