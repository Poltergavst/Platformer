public class LethalInteractor : PlayerInteractor
{
    protected override void InteractWithPlayer(Player player)
    {
        player.TakeHit(transform.position, true);
    }
}
