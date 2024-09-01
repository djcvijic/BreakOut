using UnityEngine;

[CreateAssetMenu]
public class PowerUpScriptable : ScriptableObject
{
    public static PowerUpScriptable Load(int assetName)
    {
        return Resources.Load<PowerUpScriptable>($"PowerUps/{assetName}");
    }
}