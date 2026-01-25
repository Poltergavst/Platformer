using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VampirismView : MonoBehaviour
{
    [SerializeField] private Slider _chargeBar;
    [SerializeField] private Vampirism _vampire;
    [SerializeField] private SpriteRenderer _range;
    [SerializeField] private SpriteRenderer _button;

    private bool _isAbilityActive;

    private Coroutine _runningCoroutine;

    private void OnEnable()
    {
        _vampire.ChargeChanged += OnChargeChanged;
        _vampire.AbilityStarted += OnAbilityStarted;
        _vampire.RechargeStarted += OnRechargeStarted;
        _vampire.RechargeEnded += OnRechargeEnded;
        _vampire.GotReset += ResetView;
    }

    private void OnDisable()
    {
        _vampire.ChargeChanged -= OnChargeChanged;
        _vampire.AbilityStarted -= OnAbilityStarted;
        _vampire.RechargeStarted -= OnRechargeStarted;
        _vampire.RechargeEnded -= OnRechargeEnded;
        _vampire.GotReset -= ResetView;
    }

    private void Start()
    {
        ResetView();

        HideSprite(_range);
    }

    private void ResetView()
    {
        _chargeBar.value = _chargeBar.maxValue;

        if (_runningCoroutine != null )
        {
            CoroutineRunner.Instance.StopCoroutine(_runningCoroutine);
        }

        HideSprite(_range);
    }

    private void UpdateMaxValue(float value)
    {
        if(_chargeBar.value == _chargeBar.maxValue)
        {
            _chargeBar.maxValue = value;
            _chargeBar.value = value;
        }
        else
        {
            _chargeBar.maxValue = value;
        }
    }

    private void OnChargeChanged(float amount)
    {
        _chargeBar.value += amount;
    }

    private void OnAbilityStarted(float value)
    {
        _isAbilityActive = true;

        HideSprite(_button);

        UpdateMaxValue(value);

        _runningCoroutine = CoroutineRunner.Instance.StartCoroutine(DrawRange());
    }

    private void OnRechargeStarted(float value)
    {
        _isAbilityActive = false;

        UpdateMaxValue(value);
    }

    private void OnRechargeEnded()
    {
        ShowSprite(_button);
    }

    private void ShowSprite(SpriteRenderer renderer)
    {
        renderer.enabled = true;
    }

    private void HideSprite(SpriteRenderer renderer)
    {
        renderer.enabled = false;
    }

    private IEnumerator DrawRange()
    {
        _range.transform.localScale = _vampire.AbilityRadius * Vector2.one;

        ShowSprite(_range);

        while (_isAbilityActive)
        {
            _range.transform.position = transform.position;

            yield return null;
        }

        HideSprite(_range);
    }
}
