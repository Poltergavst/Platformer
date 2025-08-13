using UnityEngine;

public class InputReader : MonoBehaviour
{
    private const string Horizontal = nameof(Horizontal);
    private const string Jump = nameof(Jump);
    private const string Attack = nameof(Attack);
   
    private bool _isJump;
    private bool _isAttack;

    public float Direction { get; private set; }

    private void Update()
    {
        Direction = Input.GetAxis(Horizontal);

        if (Input.GetButtonDown(Jump))
        {
            _isJump = true;
        }
        
        if (Input.GetButtonDown(Attack))
        {
            _isAttack = true;
        }
    }

    public bool IsJumpPressed() => GetBoolAsTrigger(ref _isJump);
    public bool IsAttackPressed() => GetBoolAsTrigger(ref _isAttack);


    private bool GetBoolAsTrigger(ref bool value)
    {
        bool localValue = value;
        
        value = false;

        return localValue;
    }
}
