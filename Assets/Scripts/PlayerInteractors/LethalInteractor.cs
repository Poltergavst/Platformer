public class LethalInteractor : PlayerInteractor
{
    protected override void InteractWithPlayer(Player player)
    {
        if (player.TryGetComponent(out Health health))
        {
            player.TakeDamage(transform.position, health.Current);
        }
    }
}
