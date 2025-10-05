using UnityEngine;

public class CoroutineRunnerForInactives : MonoBehaviour
{
    private static CoroutineRunnerForInactives _instance;

    public static CoroutineRunnerForInactives Instance
    {
        get
        {
            if ( _instance == null )
                _instance = FindObjectOfType<CoroutineRunnerForInactives>();

            if (_instance == null)
                _instance = new GameObject(nameof(CoroutineRunnerForInactives)).AddComponent<CoroutineRunnerForInactives>();

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }
}
