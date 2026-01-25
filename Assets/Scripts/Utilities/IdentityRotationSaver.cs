using UnityEngine;

public class IdentityRotationSaver : MonoBehaviour
{
    [SerializeField] private Rotator _rotator;

    private Quaternion _identityRotation;

    private void OnEnable()
    {
        _rotator.Turned += ReturnRotation;
    }

    private void OnDisable()
    {
        _rotator.Turned -= ReturnRotation;
    }

    private void Start()
    {
       _identityRotation = Quaternion.identity;
    }

    private void ReturnRotation()
    {
        transform.rotation = _identityRotation;
    }
}
