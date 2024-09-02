using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BrickScriptable : ScriptableObject
{
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private List<HealthMaterial> healthMaterials;

    [field: SerializeField] public int StartingHealth { get; private set; } = 1;
    [field: SerializeField] public bool IsInvincible { get; private set; }
    [field: SerializeField] public bool DoesTeleport { get; private set; }
    [field: SerializeField] public int ScoreContribution { get; private set; } = 1;

    public static BrickScriptable Load(int assetName)
    {
        return Resources.Load<BrickScriptable>($"Bricks/{assetName}");
    }

    public Material GetMaterial(int currentHealth)
    {
        if (healthMaterials == null) return defaultMaterial;

        var material = healthMaterials.Find(x => x.Health == currentHealth);
        return material?.Material ?? defaultMaterial;
    }
}

[Serializable]
public class HealthMaterial
{
    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] public Material Material { get; private set; }
}