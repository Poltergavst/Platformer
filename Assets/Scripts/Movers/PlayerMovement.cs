using System;
using UnityEngine;

[RequireComponent(typeof(GroundChecker))]
public class PlayerMovement : Mover
{
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _fallGravityMultiplier;

    private InputReader _inputReader;
    private GroundChecker _groundChecker;

    private bool _isJumping;

    private int _stepsSinceLastJump;
    private int _stepsSinceLastGrounded;

    public event Action<int> Ran;
    public event Action<int> Stopped;
    public event Action<int> Jumped;
    public event Action<int> Fell;

    protected override void Awake()
    {
        _inputReader = GetComponent<InputReader>();
        _groundChecker = GetComponent<GroundChecker>();

        base.Awake();
    }

    private void OnEnable()
    {
        _inputReader.enabled = true;
    }

    private void OnDisable()
    {
        _inputReader.enabled = false;
    }

    private void FixedUpdate()
    {
        _stepsSinceLastGrounded++;
        _stepsSinceLastJump++;

        Move();

        Rotator.Turn(Vector2.zero.x, _inputReader.Direction);

        _isJumping = _inputReader.IsJumpPressed();

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
        Rigidbody.velocity = Rigidbody.velocity.Change(y: _jumpForce);
    }

    public override void Move()
    {
        Rigidbody.velocity = Rigidbody.velocity.Change(x: _inputReader.Direction * Speed);
    }

    private void PerformGroundedActions(RaycastHit2D ground)
    {
        _stepsSinceLastGrounded = 0;

        if (_stepsSinceLastJump > 1) 
        {
            if (_inputReader.Direction != 0)
            {
                Ran?.Invoke(PlayerAnimatorStates.PlayerRun);
            }
            else
            {
                Stopped?.Invoke(PlayerAnimatorStates.PlayerIdle);
            }
        }

        if(_isJumping)
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
        if (Rigidbody.velocity.y < 0)
        {
            Fell?.Invoke(PlayerAnimatorStates.PlayerFall);

            Rigidbody.velocity += (_fallGravityMultiplier - 1) * Time.fixedDeltaTime * Physics2D.gravity * Vector2.up;
        }
    }
    
    private void HandleSlopes(RaycastHit2D ground)
    {
        if (IsOnSlope(ground) && _inputReader.IsJumpPressed() == false && _inputReader.Direction == 0)
        {
            Rigidbody.gravityScale = 0;
            Rigidbody.velocity = Vector3.zero;
        }
        else
        {
            Rigidbody.gravityScale = 1;
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

        RaycastHit2D hitBelow = Physics2D.Raycast(Rigidbody.position, Vector2.down);

        if (hitBelow)
        {
            _stepsSinceLastGrounded = 0;

            float speed = Rigidbody.velocity.magnitude;
            float projectedDistance = Vector2.Dot(Rigidbody.velocity, hitBelow.normal);

            if (projectedDistance > 0f)
            {
                Rigidbody.velocity = (hitBelow.normal * projectedDistance).DirectionTo(Rigidbody.velocity) * speed;
            }
        }
    }
}
