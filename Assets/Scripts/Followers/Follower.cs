using UnityEngine;

public abstract class Follower : MonoBehaviour
{
    [SerializeField] protected Transform _target;

    private Vector3 _targetPosition;

    protected virtual void FixedUpdate()
    {
        if (IsTargetOutOfReach())
        {
            SnapToTarget(_target.position);
        }

        _targetPosition = GetTargetPosition(_target.position);

        FollowTarget(transform.position, _targetPosition);
    }

    protected abstract bool IsTargetOutOfReach();
    protected abstract void FollowTarget(Vector3 currentPosition, Vector3 targetPosition);

    protected virtual Vector3 GetTargetPosition(Vector3 targetPosition)
    {
        return targetPosition;
    }

    protected void SnapToTarget(Vector3 targetPosition)
    {
        transform.position = GetTargetPosition(targetPosition);
    }
}

