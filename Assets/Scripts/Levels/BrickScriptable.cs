using UnityEngine;

[CreateAssetMenu]
public class BrickScriptable : ScriptableObject
{
    public static BrickScriptable Load(int assetName)
    {
        return Resources.Load<BrickScriptable>($"Bricks/{assetName}");
    }
}