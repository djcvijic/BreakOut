using UnityEngine;

public class GameBounds : MonoSingleton<GameBounds>
{
    [SerializeField] private Transform shape;

    public Transform BoundsTransform => shape;
}
