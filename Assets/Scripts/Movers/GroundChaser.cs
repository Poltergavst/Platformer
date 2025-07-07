using System;
using UnityEngine;

public class GroundChaser : GroundMover
{
    [SerializeField] private float _sightDistance;
    [SerializeField] private Transform _target;

    private bool _isTargetInSight;
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
        _isTargetInSight = IsTargetInSight();
        Debug.Log(_isTargetInSight);
    }

    private bool IsTargetInSight()
    {
        Vector3 targetPosition = _target.position;
        Vector3 searcherPosition = transform.position;

        _sightDistance = 3f;

        //GroundChecker.

        if (searcherPosition.SqrDistanceTo(targetPosition) < _sightDistance * _sightDistance)
        {
            if (IsFacingTarget(targetPosition, searcherPosition))
            {
                if (_isTargetInSight == false)
                    TargetDetected?.Invoke();
            }

            return true;
        }

        if (_isTargetInSight)
            TargetLost?.Invoke();

        return false;
    }
    private bool IsFacingTarget(Vector2 targetPosition, Vector2 searcherPosition)
    {
        bool IsTargetOnTheRight;

        IsTargetOnTheRight = targetPosition.x > searcherPosition.x;

        return IsTargetOnTheRight == Rotator.IsFacingRight;
    }
}
