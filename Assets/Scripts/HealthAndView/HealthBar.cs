using UnityEngine;
using UnityEngine.UI;

public class HealthBar : HealthView
{
    [SerializeField] protected Slider _slider;

    [SerializeField] private Color _fullIndicator;
    [SerializeField] private Color _middleIndicator;
    [SerializeField] private Color _lowIndicator;

    private Image _fill;

    protected override void Start()
    {
        _slider.maxValue = _health.Max;
        _slider.minValue = _health.Min;

        _fill = _slider.fillRect.gameObject.GetComponent<Image>();

        base.Start();
    }

    protected override void UpdateHealth(float amount)
    {
        _slider.value = amount;

        UpdateColor(amount);
    }

    protected void UpdateColor(float health)
    {
        float fullValue = 100f;
        float fullToMiddleBorder = 70f;
        float middleToLowBorder = 35f;

        float percent = _health.Max / fullValue;

        if (health > fullToMiddleBorder * percent)
        {
            _fill.color = _fullIndicator;
        }
        else
        {
            if(health <= middleToLowBorder * percent)
            {
                _fill.color = _lowIndicator;
                return;
            }
            else
            {
                _fill.color = _middleIndicator;
            }
        }
    }
}
