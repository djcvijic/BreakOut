using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Fireball")]
public class FireballPowerUp : PowerUpScriptable
{
    private Ball _ball;

    public override bool HasDuration => true;

    protected override void Run()
    {
        _ball = GameController.Instance.AnyBall;
        _ball.IsOnFire = true;
    }

    protected override void OnEnded()
    {
        _ball.IsOnFire = false;
    }
}
