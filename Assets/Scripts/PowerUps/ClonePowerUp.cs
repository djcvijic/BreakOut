using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Clone")]
public class ClonePowerUp : PowerUpScriptable
{
    [field: SerializeField] public int BallsToAdd { get; private set; } = 1;

    public override bool HasDuration => false;

    public void Run()
    {
        var ball = GameController.Instance.AnyBall;
        for (var i = 0; i < BallsToAdd; i++)
        {
            Instantiate(ball).UnAttach();
        }
    }
}