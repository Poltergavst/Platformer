using UnityEngine;

[RequireComponent(typeof(PlayerDetector))]
public abstract class PlayerInteractor : MonoBehaviour
{
    protected PlayerDetector PlayerDetector;

    private void Awake()
    {
        PlayerDetector = GetComponent<PlayerDetector>();
    }

    private void OnEnable()
    {
        PlayerDetector.PlayerDetected += InteractWithPlayer;
    }

    private void OnDisable()
    {
        PlayerDetector.PlayerDetected -= InteractWithPlayer;
    }

    protected abstract void InteractWithPlayer(Player player);
}
