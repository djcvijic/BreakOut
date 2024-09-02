using UnityEngine;

public abstract class PowerUpScriptable : ScriptableObject
{
    [field: SerializeField] public int DurationSeconds { get; private set; }

    public abstract bool HasDuration { get; }

    public static PowerUpScriptable Load(string assetName)
    {
        return Resources.Load<PowerUpScriptable>($"PowerUps/{assetName}");
    }
}