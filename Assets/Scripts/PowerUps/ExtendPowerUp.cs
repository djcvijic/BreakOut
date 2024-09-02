using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Extend")]
public class ExtendPowerUp : PowerUpScriptable
{
    [SerializeField] private float scalingFactor = 2f;

    private Paddle _paddle;
    private Vector3 _originalScale;

    public override bool HasDuration => true;

    protected override void Run()
    {
        _paddle = FindAnyObjectByType<Paddle>(FindObjectsInactive.Exclude);
        var paddleTransform = _paddle.Shape;
        _originalScale = paddleTransform.localScale;
        paddleTransform.localScale = new Vector3(scalingFactor * _originalScale.x, _originalScale.y, _originalScale.z);
    }

    protected override void OnEnded()
    {
        _paddle.Shape.localScale = _originalScale;
    }
}