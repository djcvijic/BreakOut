using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Gun")]
public class GunPowerUp : PowerUpScriptable
{
    public override bool HasDuration => true;

    public override string Name => "GUN";

    protected override void Run()
    {
        Gameplay.Instance.Paddle.HasAGun = true;
    }

    protected override void OnEnded()
    {
        Gameplay.Instance.Paddle.HasAGun = false;
    }
}