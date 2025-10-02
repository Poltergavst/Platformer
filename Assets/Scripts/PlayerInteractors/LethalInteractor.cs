public class LethalInteractor : PlayerInteractor
{
    protected override void InteractWithPlayer(Player player)
    {
        if (player.TryGetComponent<Health>(out Health health))
        {
            player.TakeDamage(transform.position, health.Current);
        }
    }
}
