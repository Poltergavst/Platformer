using UnityEngine;

public class GroundChaser : GroundMover
{
    public override void Move() => Chase();

    public void SetOnTarget(Vector3 targetPosition)
    {
        Destination = targetPosition;
    }

    private void Chase()
    {
        Rotator.Turn(transform.position.x, Destination.x);

        base.Move();
    }
}