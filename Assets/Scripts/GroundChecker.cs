using System;
using System.Buffers;
using UnityEngine;

[RequireComponent (typeof(Collider2D))]
public class GroundChecker : MonoBehaviour
{
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _groundCheckDistance;

    private const int BufferSize = 1;

    private float _distanceToColliderBottom;
    private float _boxAngle;
    private Vector2 _boxSize;

    private RaycastHit2D _emptyHit;
    private RaycastHit2D[] _hits;

    private void Awake()
    {
        _emptyHit = new RaycastHit2D();
        _hits = new RaycastHit2D[BufferSize];

        InitializeBoxParameters(GetComponent<Collider2D>());
    }

    public bool IsGround(out RaycastHit2D hit, Vector2 position)
    {
        int hitsCount;
        int firstHitIndex = 0;

        hitsCount = Physics2D.BoxCastNonAlloc(position, _boxSize, _boxAngle, Vector2.down, _hits, _distanceToColliderBottom + _groundCheckDistance, _groundMask);

        if (hitsCount > 0)
        {
            hit = _hits[firstHitIndex];
        }
        else 
        {
            hit = _emptyHit;
        }

        return hit.collider != null;
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
