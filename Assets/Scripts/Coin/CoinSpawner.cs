using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private int _activeCoinAmount;
    [SerializeField] private float _secondsBeforeRespawn;

    [SerializeField] private ICollectable _prefab;
    [SerializeField] private Transform _spawnpointsContainer;
    [SerializeField] private Vector2[] _spawnPositions;
    [SerializeField] private LayerMask _dontSpawnOnTop;

    private ICollectable[] _coins;
    private List<ICollectable> _unactiveCoins;

    private delegate void CoinDelegate(ICollectable coin);

    private void Awake()
    {
        int minCoinAmount = 0;

        _unactiveCoins = new List<ICollectable>();

        Mathf.Clamp(_activeCoinAmount, minCoinAmount, _spawnPositions.Length);

        CreateCoinsAtSpawnpoints();
    }

    //private void OnEnable() 
    //{
    //    DoToAllCoins(Subscribe);
    //}

    //private void OnDisable()
    //{
    //    DoToAllCoins(Unsubscribe);
    //}

    private void Start()
    {
        InitialSpawn();
    }

    private void CreateCoinsAtSpawnpoints()
    {
        int amountOfCoin = _spawnPositions.Length;
        Transform coinsCointainer = new GameObject("Coins").transform;

        _coins = new ICollectable[amountOfCoin];

        for (int i = 0; i < amountOfCoin; i++)
        {
            _coins[i] = Instantiate(_prefab, _spawnPositions[i], Quaternion.identity, coinsCointainer);

            Deactivate(_coins[i]);
        }
    }
    private ICollectable PickRandomCoin()
    {
        int minValue = 0;
        int maxValue = _unactiveCoins.Count;

        if (_unactiveCoins.Count == 0)
        {
            return null;
        }

        return _unactiveCoins[Random.Range(minValue, maxValue)];
    }

    private void InitialSpawn()
    {
        for (int i = 0; i < _activeCoinAmount; i++)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        float searchRadius = 1f;

        ICollectable coin = PickRandomCoin();

        if (coin != null)
        {
            bool isSpawnpointOccupied = Physics2D.OverlapCircle(coin.transform.position, searchRadius, _dontSpawnOnTop);

            if (isSpawnpointOccupied)
            {
                StartCoroutine(Respawn());
                return;
            }
        }

        Activate(coin);
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

    private void Activate(ICollectable coin)
    {
        if (coin != null)
        {
            _unactiveCoins.Remove(coin);
            coin.gameObject.SetActive(true);
        }
    }

    private void Deactivate(ICollectable coin)
    {
        _unactiveCoins.Add(coin);
        coin.gameObject.SetActive(false);
    }

    //private void Subscribe(ICollectable coin)
    //{
    //    coin.Collected += Despawn;
    //}

    //private void Unsubscribe(ICollectable coin)
    //{
    //    coin.Collected -= Despawn;
    //}

    private void DoToAllCoins(CoinDelegate operation)
    {
        foreach (ICollectable coin in _coins)
        {
            operation(coin);
        }
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
