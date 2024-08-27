using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float moveSpeed = 1f;

    private void Update()
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
        var myScale = transform.lossyScale;
        var minX = boundsPosition.x - 0.5f * (boundsScale.x - myScale.x);
        var maxX = boundsPosition.x + 0.5f * (boundsScale.x - myScale.x);
        return position.x < minX || position.x > maxX
            ? new Vector3(Mathf.Clamp(position.x, minX, maxX), position.y, position.z)
            : position;
    }
}
