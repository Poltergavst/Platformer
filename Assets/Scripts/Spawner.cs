using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private ICollectable _prefab;

    [SerializeField] private int _activeAmount;
    [SerializeField] private float _secondsBeforeRespawn;

    [SerializeField] private Transform _spawnpointsContainer;
    [SerializeField] private Vector2[] _spawnPositions;
    [SerializeField] private LayerMask _dontSpawnOnTop;

    private ICollectable[] _items;
    private List<ICollectable> _unactiveItems;

    private delegate void ItemDelegate(ICollectable item);

    private void Awake()
    {
        int minItemAmount = 0;

        _unactiveItems = new List<ICollectable>();

        Mathf.Clamp(_activeAmount, minItemAmount, _spawnPositions.Length);

        CreateItemsAtSpawnpoints();
    }

    private void OnEnable()
    {
        ICollectable.Collected += Despawn;
    }

    private void OnDisable()
    {
        ICollectable.Collected -= Despawn;
    }

    private void Start()
    {
        InitialSpawn();
    }

    private void CreateItemsAtSpawnpoints()
    {
        int amountOfItems = _spawnPositions.Length;

        Transform itemsContainer = new GameObject("Items").transform;

        _items = new ICollectable[amountOfItems];

        for (int i = 0; i < amountOfItems; i++)
        {
            _items[i] = Instantiate(_prefab, _spawnPositions[i], Quaternion.identity, itemsContainer);

            Deactivate(_items[i]);
        }
    }
    private ICollectable PickRandomItem()
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
        float searchRadius = 1f;

        ICollectable item = PickRandomItem();

        if (item != null)
        {
            bool isSpawnpointOccupied = Physics2D.OverlapCircle(item.transform.position, searchRadius, _dontSpawnOnTop);

            if (isSpawnpointOccupied)
            {
                StartCoroutine(Respawn());
                return;
            }
        }

        Activate(item);
    }

    private void Despawn(ICollectable coin)
    {
        Deactivate(coin);
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        WaitForSeconds respawnDelay = new WaitForSeconds(_secondsBeforeRespawn);

        yield return respawnDelay;

        Spawn();
    }

    private void Activate(ICollectable item)
    {
        if (item != null)
        {
            _unactiveItems.Remove(item);
            item.gameObject.SetActive(true);
        }
    }

    private void Deactivate(ICollectable item)
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
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        foreach (Vector2 spawnPosition in _spawnPositions)
        {
            Gizmos.DrawSphere(spawnPosition, 0.3f);
        }

        RefreshChildArray();
    }
#endif
}
