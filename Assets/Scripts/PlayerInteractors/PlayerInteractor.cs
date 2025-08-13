using UnityEngine;

[RequireComponent(typeof(PlayerDetector))]
public abstract class PlayerInteractor : MonoBehaviour
{
    protected PlayerDetector PlayerDetector;

    protected virtual void Awake()
    {
        PlayerDetector = GetComponent<PlayerDetector>();
    }

    protected virtual void OnEnable()
    {
        PlayerDetector.PlayerDetected += InteractWithPlayer;
    }

    protected virtual void OnDisable()
    {
        PlayerDetector.PlayerDetected -= InteractWithPlayer;
    }

    protected abstract void InteractWithPlayer(Player player);
}
