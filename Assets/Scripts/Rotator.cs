using UnityEngine;

public class Rotator: MonoBehaviour
{
    public void Turn(float position, float direction)
    {
        int rotationValue;
        int rightTurnValue = 0;
        int leftTurnValue = 180;

        bool isFacingRight;

        Quaternion initialRotation = transform.rotation;

        if (direction != position)
        {
            isFacingRight = direction > position;

            rotationValue = isFacingRight ? rightTurnValue : leftTurnValue;

            transform.rotation = Quaternion.Euler(initialRotation.x, rotationValue, initialRotation.y);
        }
    }
}
