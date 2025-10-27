using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner _instance;

    public static CoroutineRunner Instance
    {
        get
        {
            if ( _instance == null )
                _instance = FindObjectOfType<CoroutineRunner>();

            if (_instance == null)
                _instance = new GameObject(nameof(CoroutineRunner)).AddComponent<CoroutineRunner>();

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
