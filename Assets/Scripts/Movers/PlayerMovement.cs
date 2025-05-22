using System;
using UnityEngine;

public class PlayerMovement : Mover
{
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _fallGravityMultiplier;

    private InputReader _inputReader;

    private int _stepsSinceLastGrounded, _stepsSinceLastJump;

    public event Action<int> Ran, Stopped, Jumped, Fell;

    protected override void Awake()
    {
        _inputReader = GetComponent<InputReader>();

        base.Awake();
    }

    protected override void FixedUpdate()
    {
        _stepsSinceLastGrounded++;
        _stepsSinceLastJump++;

        base.FixedUpdate();

        Turn(Vector2.zero.x, _inputReader.Direction);

        if (_groundChecker.IsGround(out RaycastHit2D ground, transform.position))
        {
            PerformGroundedActions(ground);
        }
        else
        {
            PerformMidAirActions();
        }
    }

    public void Jump()
    {
        Jumped?.Invoke(PlayerAnimatorStates.PlayerJump);

        _stepsSinceLastJump = 0;
        _rigidbody.velocity = _rigidbody.velocity.Change(y: _jumpForce);
    }

    protected override void Move()
    {
        _rigidbody.velocity = _rigidbody.velocity.Change(x: _inputReader.Direction * _speed);
    }

    private void PerformGroundedActions(RaycastHit2D ground)
    {
        _stepsSinceLastGrounded = 0;

        if (_inputReader.Direction != 0)
        {
            Ran?.Invoke(PlayerAnimatorStates.PlayerRun);
        }
        else
        {
            Stopped?.Invoke(PlayerAnimatorStates.PlayerIdle);
        }

        if (_inputReader.IsJumpPressed() == true)
        {
            Jump();
        }

        HandleSlopes(ground);
    }

    private void PerformMidAirActions()
    {
        StickToTheGround();
        HandleFall();
    }

    private void HandleFall()
    {
        if (_rigidbody.velocity.y < 0)
        {
            Fell?.Invoke(PlayerAnimatorStates.PlayerFall);

            _rigidbody.velocity += (_fallGravityMultiplier - 1) * Time.fixedDeltaTime * Physics2D.gravity * Vector2.up;
        }
    }
    
    private void HandleSlopes(RaycastHit2D ground)
    {
        if (IsOnSlope(ground) && _inputReader.IsJumpPressed() == false && _inputReader.Direction == 0)
        {
            _rigidbody.gravityScale = 0;
            _rigidbody.velocity = Vector3.zero;
        }
        else
        {
            _rigidbody.gravityScale = 1;
        }
    }

    private bool IsOnSlope(RaycastHit2D ground)
    {
        return Vector2.Angle(Vector2.up, ground.normal) != 0;
    }

    private void StickToTheGround()
    {
        if (_stepsSinceLastGrounded > 1 || _stepsSinceLastJump <= 2)
        {
            return;
        }

        RaycastHit2D hitBelow = Physics2D.Raycast(_rigidbody.position, Vector2.down);

        if (hitBelow)
        {
            _stepsSinceLastGrounded = 0;

            float speed = _rigidbody.velocity.magnitude;
            float projectedDistance = Vector2.Dot(_rigidbody.velocity, hitBelow.normal);

            if (projectedDistance > 0f)
            {
                _rigidbody.velocity = (hitBelow.normal * projectedDistance).DirectionTo(_rigidbody.velocity) * speed;
            }
        }
    }
}
