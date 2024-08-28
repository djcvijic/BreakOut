using UnityEngine;
using Random = System.Random;

public class Ball : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float speed = 1f;

    private bool _attached;
    private Vector3 _currentVelocity;

    public void Attach()
    {
        _attached = true;
    }

    public void UnAttach()
    {
        _attached = false;

        var random = new Random();
        var randomAngle = Mathf.PI/2 * (0.5f + (float)random.NextDouble());
        randomAngle += randomAngle > Mathf.PI/2 ? Mathf.PI/12 : -Mathf.PI/12;
        var angleVector = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0f);
        _currentVelocity = speed * angleVector;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (_attached) return;

        var myTransform = transform;
        var newPosition = myTransform.localPosition + _currentVelocity * Time.deltaTime;
        (myTransform.localPosition, _currentVelocity) = ClampWithGameBounds(newPosition);
    }

    private (Vector3 position, Vector3 velocity) ClampWithGameBounds(Vector3 position)
    {
        var boundsTransform = GameBounds.Instance.BoundsTransform;
        var boundsPosition = boundsTransform.position;
        var boundsScale = boundsTransform.lossyScale;
        var myScale = spriteRenderer.transform.lossyScale;
        var minX = boundsPosition.x - 0.5f * (boundsScale.x - myScale.x);
        var maxX = boundsPosition.x + 0.5f * (boundsScale.x - myScale.x);
        var minY = boundsPosition.y - 0.5f * (boundsScale.y - myScale.y);
        var maxY = boundsPosition.y + 0.5f * (boundsScale.y - myScale.y);
        var velocity = _currentVelocity;

        if (position.x < minX && _currentVelocity.x < 0 || position.x > maxX && _currentVelocity.x > 0)
        {
            position = new Vector3(Mathf.Clamp(position.x, minX, maxX), position.y, position.z);
            velocity = new Vector3(-_currentVelocity.x, _currentVelocity.y, _currentVelocity.z);
        }

        if (position.y < minY && _currentVelocity.y < 0 || position.y > maxY && _currentVelocity.y > 0)
        {
            position = new Vector3(position.x, Mathf.Clamp(position.y, minY, maxY), position.z);
            velocity = new Vector3(_currentVelocity.x, -_currentVelocity.y, _currentVelocity.z);
        }

        return (position, velocity);
    }
}