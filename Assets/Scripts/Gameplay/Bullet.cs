using System.Linq;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;

    public static void SpawnAt(Vector3 position)
    {
        var inactiveBullet = FindObjectsByType<Bullet>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .FirstOrDefault(x => !x.isActiveAndEnabled);
        if (inactiveBullet != null)
        {
            inactiveBullet.transform.position = position;
            inactiveBullet.gameObject.SetActive(true);
            return;
        }

        var prefab = Resources.Load<Bullet>("Prefabs/Bullet");
        Instantiate(prefab, position, Quaternion.identity);
    }

    private void Update()
    {
        if (Gameplay.Instance.CurrentState != Gameplay.State.Playing) return;

        transform.localPosition += speed * Time.deltaTime * Vector3.up;
        if (transform.position.y > GameBounds.Instance.BoundsTransform.lossyScale.y / 2)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Gameplay.Instance.CurrentState != Gameplay.State.Playing) return;

        var brick = other.GetComponentInParent<Brick>();
        if (brick != null)
        {
            brick.GetHit();
            gameObject.SetActive(false);
            return;
        }
    }
}