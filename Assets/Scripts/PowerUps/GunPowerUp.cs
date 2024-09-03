using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Gun")]
public class GunPowerUp : PowerUpScriptable
{
    public override bool HasDuration => true;

    protected override void Run()
    {
        GameController.Instance.Paddle.HasAGun = true;
    }

    protected override void OnEnded()
    {
        GameController.Instance.Paddle.HasAGun = false;
    }
}