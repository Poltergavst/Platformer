using System;
using UnityEngine;

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
        PlayState(PlayerAnimatorStates.Empty, 0);
    }

    public void ResumeAnimations()
    {
        PlayState(PlayerAnimatorStates.PlayerIdle, 0);
    }

    public void PlayState(int id, int layer)
    {
        _animator.Play(id, layer);
    }

    private void ResetFrame()
    {
        _renderer.sprite = _idleSprite;
    }
}
