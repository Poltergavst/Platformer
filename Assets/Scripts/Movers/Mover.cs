using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public abstract class Mover : MonoBehaviour
{
    [SerializeField] protected float _speed;

    protected Rigidbody2D _rigidbody;
    protected GroundChecker _groundChecker;

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _groundChecker = GetComponent<GroundChecker>();
    }

    protected virtual void FixedUpdate()
    {
        Move();
    }

    protected abstract void Move();

    protected void Turn(float turnFrom, float turnTo)
    {
        int rotationValue;
        int rightTurnValue = 0;
        int leftTurnValue = 180;

        bool isFacingRight;

        Quaternion initialRotation = transform.rotation;

        if (turnTo != turnFrom)
        {
            isFacingRight = turnTo > turnFrom;

            rotationValue = isFacingRight ? rightTurnValue : leftTurnValue;

            transform.rotation = Quaternion.Euler(initialRotation.x, rotationValue, initialRotation.y);
        }
    }
}
