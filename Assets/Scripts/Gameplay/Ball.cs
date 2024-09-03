using Messages;
using UnityEngine;
using Util;
using Random = System.Random;

public class Ball : MonoBehaviour
{
    [SerializeField] private Transform shape;
    [SerializeField] private float speed = 1f;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material fireMaterial;

    private bool _attached;
    private Vector3 _currentVelocity;
    private Material _defaultMaterial;
    private bool _isOnFire;

    public bool IsOnFire
    {
        get => _isOnFire;
        set
        {
            _isOnFire = value;
            meshRenderer.material = _isOnFire ? fireMaterial : _defaultMaterial;
        }
    }

    private void Start()
    {
        _defaultMaterial = meshRenderer.material;
    }

    public void Attach()
    {
        _attached = true;
    }

    public void UnAttach()
    {
        _attached = false;
        SetRandomAngle();
    }

    private void SetRandomAngle()
    {
        var random = new Random();
        var randomAngle = Mathf.PI / 2 * (0.5f + (float)random.NextDouble());
        randomAngle += randomAngle > Mathf.PI / 2 ? Mathf.PI / 12 : -Mathf.PI / 12;
        var angleVector = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0f);
        _currentVelocity = speed * angleVector;
    }

    private void Update()
    {
        if (GameController.Instance.CurrentState != GameController.State.Playing) return;

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
        var myScale = shape.lossyScale;
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

        if (position.y > maxY && _currentVelocity.y > 0)
        {
            position = new Vector3(position.x, Mathf.Clamp(position.y, minY, maxY), position.z);
            velocity = new Vector3(_currentVelocity.x, -_currentVelocity.y, _currentVelocity.z);
        }

        if (position.y < minY && _currentVelocity.y < 0)
        {
            gameObject.SetActive(false);
            Notifier.Instance.Notify(new BallDestroyedMessage());
        }

        return (position, velocity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GameController.Instance.CurrentState != GameController.State.Playing) return;

        var paddle = other.GetComponentInParent<Paddle>();
        if (paddle != null)
        {
            if (_currentVelocity.y > 0) return;

            SetRandomAngle();
            return;
        }

        var brick = other.GetComponentInParent<Brick>();
        if (brick != null)
        {
            var direction = brick.transform.position - transform.position;
            if (direction.y > 0 && _currentVelocity.y > 0 || direction.y < 0 && _currentVelocity.y < 0)
            {
                brick.GetHit();
                ReflectY();
            }
            else if (direction.x > 0 && _currentVelocity.x > 0 || direction.x < 0 && _currentVelocity.x < 0)
            {
                brick.GetHit();
                ReflectX();
            }

            return;
        }
    }

    private void ReflectX()
    {
        if (_isOnFire) return;

        _currentVelocity = new Vector3(-_currentVelocity.x, _currentVelocity.y, _currentVelocity.z);
    }

    private void ReflectY()
    {
        if (_isOnFire) return;

        _currentVelocity = new Vector3(_currentVelocity.x, -_currentVelocity.y, _currentVelocity.z);
    }
}