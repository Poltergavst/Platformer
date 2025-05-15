using UnityEngine;

public enum PlayerAnimatorStates
{
    PlayerIdle,
    PlayerRun,
    PlayerJump,
    PlayerFall,
    Empty
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

    public void PlayState(PlayerAnimatorStates state)
    {
        _animator.Play(state.ToString());
    }

    private void ResetFrame()
    {
        _renderer.sprite = _idleSprite;
    }
}
