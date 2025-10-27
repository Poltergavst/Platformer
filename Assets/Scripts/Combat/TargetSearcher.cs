using System;
using UnityEngine;

public class TargetSearcher : MonoBehaviour
{
    [SerializeField] private float _sightDistance;
    [SerializeField] private Transform _target;

    public event Action TargetLost;
    public event Action TargetDetected;

    public void SearchForTarget(Vector3 searchArea, Vector3 facingDirection)
    {
        Vector3 targetPosition = _target.position;

        if (IsTargetWithinArea(targetPosition, searchArea) && IsTargetInSight(targetPosition, facingDirection))
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

    private bool IsTargetWithinArea(Vector3 targetPosition, Vector3 searchingArea)
    {
        bool withinEdges;
        bool aboveGround;

        withinEdges = targetPosition.x > searchingArea.x && targetPosition.x < searchingArea.y;
        aboveGround = targetPosition.y > searchingArea.z;

        return withinEdges && aboveGround;
    }
}
