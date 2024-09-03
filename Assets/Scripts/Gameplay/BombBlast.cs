using System.Collections;
using UnityEngine;

public class BombBlast : MonoBehaviour
{
    [SerializeField] private float blastDuration = 0.2f;

    public void Initialize(float blastRadius)
    {
        transform.localScale = blastRadius * Vector3.one;
        StartCoroutine(DestroyAfterDuration());
    }

    private IEnumerator DestroyAfterDuration()
    {
        yield return new WaitForSeconds(blastDuration);
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (GameController.Instance.CurrentState != GameController.State.Playing) return;

        var brick = other.GetComponentInParent<Brick>();
        if (brick != null)
        {
            brick.Die();
            return;
        }
    }
}