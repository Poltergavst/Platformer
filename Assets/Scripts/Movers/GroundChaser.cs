using System;
using UnityEngine;

public class GroundChaser : GroundMover
{
    private bool _isChasing = false;

    public override void Move() => Chase();

    private void Chase()
    {
        SetOnTarget();

        Rotator.Turn(transform.position.x, Destination.x);

        base.Move();
    }

    private void SetOnTarget()
    {
        //Destination = _target.position;
    }
}