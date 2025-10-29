using System;
using UnityEngine;

public class TargetSearcher : MonoBehaviour
{
    [SerializeField] private float _sightDistance;
    [SerializeField] private Transform _target;

    public Transform Target => _target;

    public event Action TargetLost;
    public event Action TargetDetected;

    private Vector3 _searchArea;

    public void InitializeSearchArea(float leftEdge, float rightEdge, float bottom)
    {
        _searchArea = new Vector3(leftEdge, rightEdge, bottom);
    }

    public void SearchForTarget(Vector3 facingDirection)
    {
        Vector3 targetPosition = _target.position;

        if (IsTargetWithinArea(targetPosition) && IsTargetInSight(targetPosition, facingDirection))
        {
            TargetDetected?.Invoke();
            return;
        }

        TargetLost?.Invoke();
    }

    private bool IsTargetInSight(Vector3 targetPosition, Vector3 facingDirection)
    {
        Vector3 searcherPosition = transform.position;

        float half = 0.5f;
        float viewRadius = 3f;
        float viewAngle = 180f;

        if (searcherPosition.IsEnoughCloseTo(targetPosition, viewRadius))
        {
            if (Vector3.Angle(facingDirection, searcherPosition.DirectionTo(targetPosition)) < viewAngle * half)
            {
                return true;
            }
        }

        return false;
    }

    private bool IsTargetWithinArea(Vector3 targetPosition)
    {
        bool withinEdges;
        bool aboveGround;

        withinEdges = targetPosition.x > _searchArea.x && targetPosition.x < _searchArea.y;
        aboveGround = targetPosition.y > _searchArea.z;

        return withinEdges && aboveGround;
    }
}
