using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Slow")]
public class SlowPowerUp : PowerUpScriptable
{
    [SerializeField] private float speedMultiplier = 0.5f;

    private Ball _ball;
    private float _originalSpeed;

    public override bool HasDuration => true;

    protected override void Run()
    {
        _ball = GameController.Instance.AnyBall;
        _originalSpeed = _ball.CurrentVelocity.magnitude;
        _ball.CurrentVelocity *= speedMultiplier;
    }

    protected override void OnEnded()
    {
        _ball.CurrentVelocity = _originalSpeed * _ball.CurrentVelocity.normalized;
    }
}