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
        myTransform.localPosition = newPosition;
    }
}