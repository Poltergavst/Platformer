using UnityEngine;

public abstract class Follower : MonoBehaviour
{
    [SerializeField] protected float _smoothTime;
    [SerializeField] protected Transform _target;
    [SerializeField] protected LevelBounds _levelBounds;

    protected Vector2 MinBounds;
    protected Vector2 MaxBounds;

    protected Vector3 Velocity;

    private Vector3 _targetPosition; 

    protected virtual void Awake()
    {
        Velocity = Vector3.zero;
    }

    protected virtual void Start()
    {
        SetBounds();
        SnapToTarget(_target.position);
    }

    protected virtual void FixedUpdate()
    {
        if (IsTargetOutOfReach())
        {
            SnapToTarget(_target.position);
        }

        _targetPosition = GetTargetPosition(_target.position);
    }

    protected virtual void LateUpdate()
    {
        FollowTarget(transform.position, _targetPosition, _smoothTime);
    }

    protected abstract bool IsTargetOutOfReach();

    protected virtual Vector3 GetTargetPosition(Vector3 targetPosition)
    {
        return KeepInBounds(targetPosition);
    }

    protected virtual void SetBounds()
    {
        MinBounds = _levelBounds.MinBounds;
        MaxBounds = _levelBounds.MaxBounds;
    }

    protected void FollowTarget(Vector3 currentPosition, Vector3 targetPosition, float smoothTime)
    {
        transform.position = Vector3.SmoothDamp(currentPosition, targetPosition, ref Velocity, smoothTime);
    }

    protected Vector3 KeepInBounds(Vector3 position)
    {
        Vector3 boundedPosition;

        float clampedX = Mathf.Clamp(position.x, MinBounds.x, MaxBounds.x);
        float clampedY = Mathf.Clamp(position.y, MinBounds.y, MaxBounds.y);

        boundedPosition = new Vector3(clampedX, clampedY, transform.position.z);

        return boundedPosition;
    }

    protected void SnapToTarget(Vector3 targetPosition)
    {
        transform.position = GetTargetPosition(targetPosition);
    }
}
