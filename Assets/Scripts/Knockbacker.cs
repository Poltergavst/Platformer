using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Knockbacker : MonoBehaviour
{
    [SerializeField]  private float _duration;
    [SerializeField]  private int _strength;

    private Rigidbody2D _rigidbody;
    private Vector2 _direction;

    private Coroutine _coroutine;

    public event Action KnockbackEnded;
    public event Action KnockbackStarted;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void GetKnockbacked(Vector3 senderPosition)
    {
        KnockbackStarted?.Invoke();

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _rigidbody.velocity = Vector3.zero;

        _direction = (transform.position - senderPosition).normalized;
        _rigidbody.AddForce(_direction * _strength, ForceMode2D.Impulse);

        RestartCoroutine();
    }

    private void RestartCoroutine()
    {
        _coroutine = StartCoroutine(ResetKnockback());
    }
    
    private IEnumerator ResetKnockback()
    {
        WaitForSeconds wait = new WaitForSeconds(_duration);

        yield return wait;

        KnockbackEnded?.Invoke();
    }
}
