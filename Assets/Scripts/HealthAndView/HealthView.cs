using UnityEngine;

public abstract class HealthView : MonoBehaviour
{
    [SerializeField] protected Health _health;

    protected virtual void OnEnable()
    {
        _health.Changed += UpdateHealth;
    }

    protected virtual void OnDisable()
    {
        _health.Changed -= UpdateHealth;
    }

    protected virtual void Start()
    {
        UpdateHealth(_health.Current);
    }

    protected abstract void UpdateHealth(float amount);
}
