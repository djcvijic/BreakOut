using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private Transform shape;

    public void GetHit()
    {
        // todo
        Destroy(gameObject);
    }
}
