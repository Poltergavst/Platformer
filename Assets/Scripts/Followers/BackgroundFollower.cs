using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]
public class BackgroundFollower : ConstantFollower
{
    [SerializeField] private Camera _camera;

    private Vector2 _cameraHalfSize;
    private Vector2 _spriteHalfSize;

    private Vector2 _oldPosition;
    private SpriteRenderer _spriteRenderer;

    protected override void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _cameraHalfSize = new Vector2(_camera.orthographicSize * _camera.aspect, _camera.orthographicSize);
        _spriteHalfSize = _spriteRenderer.bounds.extents;

        base.Awake();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        _oldPosition = _target.position;

        KeepInCameraView();
    }

    protected override void SetBounds()
    {
        MinBounds = _levelBounds.MinBounds + _spriteHalfSize;
        MaxBounds = _levelBounds.MaxBounds - _spriteHalfSize;
    }

    protected override bool IsTargetOutOfReach()
    {
        float maxDistance = _cameraHalfSize.x;

        return _oldPosition.SqrDistanceTo(_target.position) > maxDistance * maxDistance;
    }

    protected override Vector3 GetTargetPosition(Vector3 targetPosition)
    {
        return base.GetTargetPosition(targetPosition);
    }

    private void KeepInCameraView()
    {
        float offset = 0.1f;

        Vector2 minBounds = _camera.transform.position.Add(x: -_cameraHalfSize.x - offset, y: -_cameraHalfSize.y - offset);
        Vector2 maxBounds = _camera.transform.position.Add(x: _cameraHalfSize.x + offset, y: _cameraHalfSize.y + offset);

        Vector2 minPosition = transform.position.Add(x: -_spriteHalfSize.x, y: -_spriteHalfSize.y);
        Vector2 maxPosition = transform.position.Add(x: _spriteHalfSize.x, y: _spriteHalfSize.y);

        if (minPosition.x > minBounds.x)
        {
            transform.position = transform.position.Add(x: -(minPosition.x - minBounds.x));
        }

        if(minPosition.y > minBounds.y)
        {
            transform.position = transform.position.Add(y: -(minPosition.y - minBounds.y));
        }

        if(maxPosition.x < maxBounds.x)
        {
            transform.position = transform.position.Add(x: maxBounds.x - maxPosition.x);
        }

        if(maxPosition.y < maxBounds.y)
        {
            transform.position = transform.position.Add(y: maxBounds.y - maxPosition.y);
        }
    }
}
