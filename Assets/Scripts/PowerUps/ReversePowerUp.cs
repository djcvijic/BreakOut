using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Reverse")]
public class ReversePowerUp : PowerUpScriptable
{
    public override bool HasDuration => true;

    protected override void Run()
    {
        PlayerInputHandler.Instance.IsReversed = true;
    }

    protected override void OnEnded()
    {
        PlayerInputHandler.Instance.IsReversed = false;
    }
}