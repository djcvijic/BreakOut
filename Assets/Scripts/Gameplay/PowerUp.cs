using Messages;
using UnityEngine;
using Util;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 1f;
    [SerializeField] private float rotationSpeed = 1f;

    private PowerUpScriptable _scriptable;

    public void Initialize(PowerUpScriptable scriptable)
    {
        _scriptable = scriptable;
    }

    private void Update()
    {
        if (Gameplay.Instance.CurrentState != Gameplay.State.Playing) return;

        transform.localPosition += fallSpeed * Time.deltaTime * Vector3.down;
        transform.rotation *= Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, Vector3.forward);
        if (transform.position.y < -GameBounds.Instance.BoundsTransform.lossyScale.y / 2)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Gameplay.Instance.CurrentState != Gameplay.State.Playing) return;

        var paddle = other.GetComponentInParent<Paddle>();
        if (paddle != null)
        {
            Notifier.Instance.Notify(new PowerUpCollectedMessage(_scriptable));
            gameObject.SetActive(false);
            return;
        }
    }
}