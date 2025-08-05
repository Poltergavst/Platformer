public class LethalInteractor : PlayerInteractor
{
    protected override void InteractWithPlayer(Player player)
    {
        player.TakeDamage(transform.position, 0, true);
    }
}
