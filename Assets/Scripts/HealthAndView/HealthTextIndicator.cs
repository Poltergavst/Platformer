using TMPro;
using UnityEngine;

public class HealthTextIndicator : HealthView
{
    [SerializeField] private TMP_Text _text;

    protected override void UpdateHealth(float amount)
    {
        _text.text = amount + "/" + _health.Max;
    }
}
