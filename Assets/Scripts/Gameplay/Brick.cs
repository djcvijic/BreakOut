using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private Transform shape;

    private BrickScriptable _scriptable;

    public Transform Shape => shape;

    public void GetHit()
    {
        // todo
        Destroy(gameObject);
    }

    public void Initialize(BrickScriptable scriptable)
    {
        _scriptable = scriptable;
    }
}
