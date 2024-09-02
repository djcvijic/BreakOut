using Messages;
using UnityEngine;
using Util;
using Random = System.Random;

public class Brick : MonoBehaviour
{
    [SerializeField] private Transform shape;
    [SerializeField] private MeshRenderer meshRenderer;

    private BrickScriptable _scriptable;
    private int _currentHealth;

    public Transform Shape => shape;
    public bool IsInvincible => _scriptable.IsInvincible;

    public void GetHit()
    {
        if (_scriptable.IsInvincible) return;

        _currentHealth -= 1;
        if (_currentHealth <= 0)
        {
            gameObject.SetActive(false);
            Notifier.Instance.Notify(new BrickDestroyedMessage(_scriptable.ScoreContribution));
            return;
        }

        meshRenderer.material = _scriptable.GetMaterial(_currentHealth);

        if (_scriptable.DoesTeleport)
        {
            Teleport();
        }
    }

    public void Initialize(BrickScriptable scriptable)
    {
        _scriptable = scriptable;
        _currentHealth = scriptable.StartingHealth;
        meshRenderer.material = _scriptable.GetMaterial(_currentHealth);
    }

    private void Teleport()
    {
        var random = new Random();
        var boundsScale = GameBounds.Instance.BoundsTransform.lossyScale;
        var newPosition = new Vector3(
            ((float)random.NextDouble() - 0.5f) * boundsScale.x,
            ((float)random.NextDouble() - 0.5f) * boundsScale.y,
            0);
        transform.position = newPosition;
    }
}
