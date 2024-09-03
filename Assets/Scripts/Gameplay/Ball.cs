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
    private Material _defaultMaterial;
    private bool _isOnFire;

    public Vector3 CurrentVelocity { get; set; }

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
        CurrentVelocity = speed * Vector3.up;
        SetRandomAngle();
    }

    private void SetRandomAngle()
    {
        var random = new Random();
        var randomAngle = Mathf.PI / 2 * (0.5f + (float)random.NextDouble());
        randomAngle += randomAngle > Mathf.PI / 2 ? Mathf.PI / 12 : -Mathf.PI / 12;
        var angleVector = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0f);
        CurrentVelocity = CurrentVelocity.magnitude * angleVector.normalized;
    }

    private void Update()
    {
        if (Gameplay.Instance.CurrentState != Gameplay.State.Playing) return;

        Move();
    }

    private void Move()
    {
        if (_attached) return;

        var myTransform = transform;
        var newPosition = myTransform.localPosition + CurrentVelocity * Time.deltaTime;
        (myTransform.localPosition, CurrentVelocity) = ClampWithGameBounds(newPosition);
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
        var velocity = CurrentVelocity;

        if (position.x < minX && CurrentVelocity.x < 0 || position.x > maxX && CurrentVelocity.x > 0)
        {
            position = new Vector3(Mathf.Clamp(position.x, minX, maxX), position.y, position.z);
            velocity = new Vector3(-CurrentVelocity.x, CurrentVelocity.y, CurrentVelocity.z);
        }

        if (position.y > maxY && CurrentVelocity.y > 0)
        {
            position = new Vector3(position.x, Mathf.Clamp(position.y, minY, maxY), position.z);
            velocity = new Vector3(CurrentVelocity.x, -CurrentVelocity.y, CurrentVelocity.z);
        }

        if (position.y < minY && CurrentVelocity.y < 0)
        {
            gameObject.SetActive(false);
            Notifier.Instance.Notify(new BallDestroyedMessage());
        }

        return (position, velocity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Gameplay.Instance.CurrentState != Gameplay.State.Playing) return;

        var paddle = other.GetComponentInParent<Paddle>();
        if (paddle != null)
        {
            if (CurrentVelocity.y > 0) return;

            SetRandomAngle();
            return;
        }

        var brick = other.GetComponentInParent<Brick>();
        if (brick != null)
        {
            var direction = brick.transform.position - transform.position;
            if (direction.y > 0 && CurrentVelocity.y > 0 || direction.y < 0 && CurrentVelocity.y < 0)
            {
                brick.GetHit();
                ReflectY();
            }
            else if (direction.x > 0 && CurrentVelocity.x > 0 || direction.x < 0 && CurrentVelocity.x < 0)
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

        CurrentVelocity = new Vector3(-CurrentVelocity.x, CurrentVelocity.y, CurrentVelocity.z);
    }

    private void ReflectY()
    {
        if (_isOnFire) return;

        CurrentVelocity = new Vector3(CurrentVelocity.x, -CurrentVelocity.y, CurrentVelocity.z);
    }
}