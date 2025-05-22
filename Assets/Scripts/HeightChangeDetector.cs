using UnityEngine;

public class HeightChangeDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _countInDetection; 

    public Vector2 FindEdgeInDirection(Vector2 direction, GroundChecker groundChecker)
    {
        float currentHeight;
        float step = 0.1f;
        float heightThreshold = 0.1f;

        Vector2 edgePosition = transform.position;

        if (groundChecker.IsGround(out RaycastHit2D hit, edgePosition))
        {
            currentHeight = hit.point.y;

            while (IsHeightUnchanged(heightThreshold, currentHeight, GetHeightInFront(edgePosition, direction)))
            {
                edgePosition += direction * step;
            }
        }

        return edgePosition;
    }

    private bool IsHeightUnchanged(float heightThreshold, float currentHeight, float heightInFront)
    {
        return Mathf.Abs(currentHeight - heightInFront) <= heightThreshold;
    }

    private float GetHeightInFront(Vector2 origin, Vector2 direction)
    {
        RaycastHit2D frontDownHit = Physics2D.Raycast(origin, Vector2.down + direction, _countInDetection);

        return frontDownHit.point.y;
    }
}
