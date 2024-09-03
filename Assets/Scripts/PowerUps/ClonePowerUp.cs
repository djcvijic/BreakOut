using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Clone")]
public class ClonePowerUp : PowerUpScriptable
{
    [field: SerializeField] public int BallsToAdd { get; private set; } = 1;

    public override bool HasDuration => false;

    public override string Name => "CLONE";

    protected override void Run()
    {
        var ball = Gameplay.Instance.AnyBall;
        for (var i = 0; i < BallsToAdd; i++)
        {
            Instantiate(ball).UnAttach();
        }
    }

    protected override void OnEnded()
    {
    }
}