using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField] private Transform shape;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float gunCoolDown = 0.2f;

    private Vector3? _attachedBallOffset;
    private Ball _attachedBall;
    private float _gunCooldownRemaining;

    public bool HasAGun { get; set; }

    public Transform Shape => shape;

    public void Attach(Ball ball)
    {
        _attachedBall = ball;
        _attachedBall.Attach();
        _attachedBallOffset ??= _attachedBall.transform.position - transform.position;
        MoveAttachedBall();
    }

    private void Update()
    {
        if (Gameplay.Instance.CurrentState != Gameplay.State.Playing) return;

        Fire();
        Move();
        MoveAttachedBall();
        UseGun();
    }

    private void UseGun()
    {
        _gunCooldownRemaining -= Time.deltaTime;

        if (!HasAGun || !PlayerInputHandler.Instance.GunAction || _gunCooldownRemaining > 0) return;

        _gunCooldownRemaining = gunCoolDown;
        Bullet.SpawnAt(shape.position);
    }

    private void Fire()
    {
        if (!PlayerInputHandler.Instance.PrimaryAction || _attachedBall == null) return;

        _attachedBall.UnAttach();
        _attachedBall = null;
    }

    private void Move()
    {
        var myTransform = transform;
        var inputDirection = PlayerInputHandler.Instance.Direction;
        var newPosition = myTransform.localPosition + moveSpeed * inputDirection * Vector3.right;
        myTransform.localPosition = ClampWithGameBounds(newPosition);
    }

    private Vector3 ClampWithGameBounds(Vector3 position)
    {
        var boundsTransform = GameBounds.Instance.BoundsTransform;
        var boundsPosition = boundsTransform.position;
        var boundsScale = boundsTransform.lossyScale;
        var myScale = shape.lossyScale;
        var minX = boundsPosition.x - 0.5f * (boundsScale.x - myScale.x);
        var maxX = boundsPosition.x + 0.5f * (boundsScale.x - myScale.x);
        return position.x < minX || position.x > maxX
            ? new Vector3(Mathf.Clamp(position.x, minX, maxX), position.y, position.z)
            : position;
    }

    private void MoveAttachedBall()
    {
        if (_attachedBall == null) return;

        _attachedBall.transform.position = transform.position + _attachedBallOffset.Value;
    }
}
