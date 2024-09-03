using Messages;
using UnityEngine;
using Util;

[CreateAssetMenu(menuName = "PowerUps/Bomb")]
public class BombPowerUp : PowerUpScriptable
{
    [SerializeField] private float blastRadius = 3f;
    [SerializeField] private BombBlast blastPrefab;

    public override bool HasDuration => false;

    public override string Name => "BOMB";

    protected override void Run()
    {
        Notifier.Instance.Subscribe<BrickDestroyedMessage>(OnBrickDestroyed);
    }

    private void OnBrickDestroyed(BrickDestroyedMessage message)
    {
        Notifier.Instance.Unsubscribe<BrickDestroyedMessage>(OnBrickDestroyed);

        Instantiate(blastPrefab, message.brickPosition, Quaternion.identity)
            .Initialize(blastRadius);
    }

    protected override void OnEnded()
    {
    }
}