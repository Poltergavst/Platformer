using UnityEngine;

public static class PlayerAnimatorStates
{
    public static readonly int Empty = Animator.StringToHash(nameof(Empty));
    public static readonly int PlayerRun = Animator.StringToHash(nameof(PlayerRun));
    public static readonly int PlayerIdle = Animator.StringToHash(nameof(PlayerIdle));
    public static readonly int PlayerJump = Animator.StringToHash(nameof(PlayerJump));
    public static readonly int PlayerFall = Animator.StringToHash(nameof(PlayerFall));
}
