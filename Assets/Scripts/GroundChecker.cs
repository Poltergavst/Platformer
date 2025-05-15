using UnityEngine;

[RequireComponent (typeof(Collider2D))]
public class GroundChecker : MonoBehaviour
{
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _groundCheckDistance;

    private float _distanceToColliderBottom;
    private float _boxAngle;
    private Vector2 _boxSize;

    private void Awake()
    {
        InitializeBoxParameters(GetComponent<Collider2D>());
    }

    public bool TryGetGround(out RaycastHit2D hit, Vector2 position)
    {  
        hit = Physics2D.BoxCast(position, _boxSize, _boxAngle, Vector2.down, _distanceToColliderBottom + _groundCheckDistance, _groundMask);

        return hit;
    }

    private void InitializeBoxParameters(Collider2D collider)
    {
        float half = 0.5f;
        float shrinkMultiplier = 0.5f;

        _boxAngle = 0;
        _boxSize = collider.bounds.size * shrinkMultiplier;

        _distanceToColliderBottom = (collider.bounds.size.y - _boxSize.y) * half;
    }
}
