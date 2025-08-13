using System;
using UnityEngine;

public class GroundChaser : GroundMover
{
    [SerializeField] private float _sightDistance;
    [SerializeField] private Transform _target;

    public override void Move() => Chase();

    public event Action TargetDetected;
    public event Action TargetLost;

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
        IsTargetInSight();
    }

    private bool IsTargetInSight()
    {
        Vector3 targetPosition = _target.position;
        Vector3 searcherPosition = transform.position;
        Vector3 lookingDirection;

        float viewRadius = 3f;
        float viewAngle = 180f;

        if(Rotator.IsFacingRight)
        {
            lookingDirection = Vector3.right;
        }
        else
        {
            lookingDirection = Vector3.left;
        }

        if (searcherPosition.IsEnoughCloseTo(targetPosition, viewRadius) && (targetPosition.y > HeightChangeDetector.GroundHeight))
        {
            if (Vector3.Angle(lookingDirection, searcherPosition.DirectionTo(targetPosition)) < viewAngle / 2f)
            {
                TargetDetected?.Invoke();
                return true;
            }
        }

        TargetLost?.Invoke();

        return false;
    }
}