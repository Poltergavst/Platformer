using UnityEngine;

[RequireComponent (typeof(Camera))]
public class CameraFollower : ConstantFollower
{
    private Camera _camera;
    private Vector3 _offset;

    protected override void Awake()
    {
        _offset = Vector3.zero;
        _camera = GetComponent<Camera>();

        base.Awake();
    }

    protected override void FixedUpdate()
    {
        PlaceAhead();

        base.FixedUpdate();
    }

    protected override Vector3 GetTargetPosition(Vector3 targetPosition)
    {
        return KeepInBounds(targetPosition + _offset);
    }

    protected override bool IsTargetOutOfReach()
    {
        Vector3 position = transform.position.Change(z: 0);
        Vector3 targetPosition = _target.position.Change(z: 0);

        float distanceToPlayer = position.SqrDistanceTo(targetPosition);
        float distanceToCameraEdge = position.SqrDistanceTo(position.Change(x: position.x + _camera.orthographicSize * _camera.aspect));

        return distanceToPlayer > distanceToCameraEdge;
    }

    protected override void SetBounds()
    {
        float cameraHalfHeight = _camera.orthographicSize;
        float cameraHalfWidth = cameraHalfHeight * _camera.aspect;

        Vector2 cameraHalfSize = new Vector2(cameraHalfWidth, cameraHalfHeight);

        MinBounds = _levelBounds.MinBounds + cameraHalfSize;
        MaxBounds = _levelBounds.MaxBounds - cameraHalfSize;
    }

    private void PlaceAhead()
    {
        float positionAhead;
        float distance = 1.1f;

        positionAhead = _target.rotation.y < 0 ? -distance : distance;

        _offset = _offset.Change(x: positionAhead);
    }
}
