using UnityEngine;

public class GameBounds : MonoSingleton<GameBounds>
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public Transform BoundsTransform => spriteRenderer.transform;
}
