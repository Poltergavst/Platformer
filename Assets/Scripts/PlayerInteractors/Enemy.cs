using UnityEngine;

public class Enemy : PlayerInteractor
{
    protected override void InteractWithPlayer(Player player)
    {
        if (IsContactFromAbove(player.transform.position))
        {
            player.GetVerticalBoost();
        }
        else
        {
            player.TakeHit(transform.position, false);
        }
    }

    private bool IsContactFromAbove(Vector3 collidedObjectPosition)
    {
        float maxDeviation = 40;
        float deviationFromTop = Vector3.Angle(transform.up, transform.position.DirectionTo(collidedObjectPosition));

        return Mathf.Abs(deviationFromTop) < maxDeviation;
    }
}
