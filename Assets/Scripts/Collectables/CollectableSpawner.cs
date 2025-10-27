using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollectableSpawner<T> : MonoBehaviour where T: MonoBehaviour, ICollectable
{
    [SerializeField] private T _prefab;

    [SerializeField] private int _activeAmount;
    [SerializeField] private float _secondsBeforeRespawn;

    [SerializeField] private Vector2[] _spawnPositions;
    [SerializeField] private LayerMask _dontSpawnOnTop;
    [SerializeField] private Transform _spawnpointsContainer;

    private T[] _items;
    private List<T> _unactiveItems;

    private void Awake()
    {
        int minItemAmount = 0;

        _unactiveItems = new List<T>();

        Mathf.Clamp(_activeAmount, minItemAmount, _spawnPositions.Length);

        CreateItemsAtSpawnpoints();
    }

    private void Start()
    {
        InitialSpawn();
    }

    protected void Despawn(T item)
    {
        Deactivate(item);
        CoroutineRunner.Instance.StartCoroutine(Respawn());
    }

    private void CreateItemsAtSpawnpoints()
    {
        int amountOfItems = _spawnPositions.Length;

        Transform itemsContainer = new GameObject("Items").transform;

        _items = new T[amountOfItems];

        for (int i = 0; i < amountOfItems; i++)
        {
            _items[i] = Instantiate(_prefab, _spawnPositions[i], Quaternion.identity, itemsContainer);

            Deactivate(_items[i]);
        }
    }

    private T PickRandomItem()
    {
        int minValue = 0;
        int maxValue = _unactiveItems.Count;

        if (_unactiveItems.Count == 0)
        {
            return null;
        }

        return _unactiveItems[Random.Range(minValue, maxValue)];
    }

    private void InitialSpawn()
    {
        for (int i = 0; i < _activeAmount; i++)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        int maxAttempts = 10;
        float searchRadius = 1.5f;

        for (int i = 0; i <= maxAttempts; i++)
        {
            T item = PickRandomItem();

            if (item != null)
            {
                bool isSpawnpointOccupied = Physics2D.OverlapCircle(item.transform.position, searchRadius, _dontSpawnOnTop);

                if (isSpawnpointOccupied == false)
                {
                    Activate(item);
                    return;
                }
            }
        }

        CoroutineRunner.Instance.StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        WaitForSeconds respawnDelay = new WaitForSeconds(_secondsBeforeRespawn);

        yield return respawnDelay;

        Spawn();
    }

    private void Activate(T item)
    {
        if (item != null)
        {
            _unactiveItems.Remove(item);
            item.gameObject.SetActive(true);
        }
    }

    private void Deactivate(T item)
    {
        _unactiveItems.Add(item);
        item.gameObject.SetActive(false);
    }

#if UNITY_EDITOR
    [ContextMenu("Refresh Child Array")]
    private void RefreshChildArray()
    {
        int pointCount = _spawnpointsContainer.childCount;

        _spawnPositions = new Vector2[pointCount];

        for (int i = 0; i < pointCount; i++)
            _spawnPositions[i] = _spawnpointsContainer.GetChild(i).position;

        Gizmos.color = Random.ColorHSV();
    }

    private void OnDrawGizmos()
    {
        foreach (Vector2 spawnPosition in _spawnPositions)
        {
            Gizmos.DrawSphere(spawnPosition, 0.3f);
        }

        RefreshChildArray();
    }
#endif
}
