using System;
using UnityEngine;

public class GroundChaser : GroundMover
{
    [SerializeField] private float _sightDistance;
    [SerializeField] private Transform _target;

    private bool _isChasing = false;

    public override void Move() => Chase();

    public event Action TargetLost;
    public event Action TargetDetected;

    private void Chase()
    {
        SetOnTarget();

        Rotator.Turn(transform.position.x, Destination.x);

        base.Move();
    }

    private void SetOnTarget()
    {
        Destination = _target.position;
    }

    private void FixedUpdate()
    {
        SearchForTarget();
    }

    private void SearchForTarget()
    {
        Vector3 targetPosition = _target.position;

        if (IsTargetAboveGround(targetPosition) && IsTargetWithinEdges(targetPosition) && IsTargetInSight(targetPosition))
        {
            if (_isChasing == false)
                TargetDetected?.Invoke();

            _isChasing = true;

            return;
        }

        if (_isChasing)
            TargetLost?.Invoke();

        _isChasing = false;
    }

    private bool IsTargetInSight(Vector3 targetPosition)
    {
        Vector3 searcherPosition = transform.position;

        Vector3 lookingDirection;

        float viewRadius = 3f;
        float lookingAngle = 60f;
        float searchingAngle = 160f;

        float viewAngle = 180f;

        viewAngle = _isChasing ? searchingAngle : lookingAngle;

        lookingDirection = Rotator.IsFacingRight ? Vector3.right : Vector3.left;

        if (searcherPosition.IsEnoughCloseTo(targetPosition, viewRadius))
        {
            if (Vector3.Angle(lookingDirection, searcherPosition.DirectionTo(targetPosition)) < viewAngle / 2f)
            {
                return true;
            }
        }

        return false;
    }

    private bool IsTargetWithinEdges(Vector3 targetPosition)
    {
        float offset = 1f;

        return targetPosition.x < RightEdge.x + offset && _target.position.x > LeftEdge.x - offset;
    }

    private bool IsTargetAboveGround(Vector3 targetPosition)
    {
        return targetPosition.y > HeightChangeDetector.GroundHeight;
    }
}