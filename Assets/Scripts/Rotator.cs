using UnityEngine;

public class Rotator: MonoBehaviour
{
    public bool IsFacingRight { get; private set; }

    public void Turn(float position, float direction)
    {
        int rotationValue;
        int rightTurnValue = 0;
        int leftTurnValue = 180;

        Quaternion initialRotation = transform.rotation;

        if (direction != position)
        {
            IsFacingRight = direction > position;

            rotationValue = IsFacingRight ? rightTurnValue : leftTurnValue;

            transform.rotation = Quaternion.Euler(initialRotation.x, rotationValue, initialRotation.y);
        }
    }
}
