using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Fireball")]
public class FireballPowerUp : PowerUpScriptable
{
    private Ball[] _balls;

    public override bool HasDuration => true;

    protected override void Run()
    {
        _balls = FindObjectsByType<Ball>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (var ball in _balls)
        {
            ball.IsOnFire = true;
        }
    }

    protected override void OnEnded()
    {
        foreach (var ball in _balls)
        {
            ball.IsOnFire = false;
        }
    }
}
