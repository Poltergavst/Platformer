using UnityEngine;

public static class PlayerAnimatorStates
{
    public static readonly int Empty = Animator.StringToHash(nameof(Empty));
    public static readonly int PlayerRun = Animator.StringToHash(nameof(PlayerRun));
    public static readonly int PlayerIdle = Animator.StringToHash(nameof(PlayerIdle));
    public static readonly int PlayerJump = Animator.StringToHash(nameof(PlayerJump));
    public static readonly int PlayerFall = Animator.StringToHash(nameof(PlayerFall));
}

[RequireComponent (typeof(Animator), typeof(SpriteRenderer))]
public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField] private Sprite _idleSprite;

    private Animator _animator;
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();

        ResumeAnimations();
    }

    public void StopAnimations()
    {
        ResetFrame();
        PlayState(PlayerAnimatorStates.Empty);
    }

    public void ResumeAnimations()
    {
        PlayState(PlayerAnimatorStates.PlayerIdle);
    }

    public void PlayState(int id)
    {
        _animator.Play(id);
    }

    private void ResetFrame()
    {
        _renderer.sprite = _idleSprite;
    }
}
