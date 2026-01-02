using UnityEngine;

public class HealthBarSmooth : HealthBar
{
    [SerializeField] private float _changingSpeed = 0.3f;

    private float _targetAmount;

    protected override void OnEnable()
    {
        _health.Changed += UpdateHealth;

        _health.Reset += SnapToMax;
    }

    protected override void OnDisable()
    {
        _health.Changed -= UpdateHealth;

        _health.Reset -= SnapToMax;
    }

    protected override void Start()
    {
        base.Start();

        _slider.value = _health.Current;
    }

    private void Update()
    {
        MoveToTargetHealth();
    }

    protected override void UpdateHealth(float amount)
    {
        _targetAmount = amount;
    }

    private void MoveToTargetHealth()
    {
        float speedDelta = _changingSpeed * Time.deltaTime;

        float step = Mathf.Lerp(_slider.value, _targetAmount, speedDelta);

        float remainingDistance = Mathf.Abs(step - _slider.value);

        if (remainingDistance < speedDelta)
        {
            step = Mathf.MoveTowards(_slider.value, _targetAmount, speedDelta);
        }

        _slider.value = step;

        UpdateColor(_slider.value);
    }

    private void SnapToMax()
    {
        _slider.value = _health.Max;
        _targetAmount = _health.Max;

        UpdateColor(_slider.value);
    }
}
