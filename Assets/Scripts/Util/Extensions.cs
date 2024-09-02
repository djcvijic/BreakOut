using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = System.Random;

public static class Extensions
{
    public static void ClearTransform(this Transform transform, params Transform[] except)
    {
        foreach (Transform child in transform)
        {
            if (except.Contains(child)) continue;

            Object.Destroy(child.gameObject);
        }
    }

    public static void SetListener(this Button button, UnityAction call)
    {
        button.onClick.SetListener(call);
    }

    public static void SetListener(this UnityEvent unityEvent, UnityAction call)
    {
        unityEvent.RemoveAllListeners();
        unityEvent.AddListener(call);
    }

    public static T Random<T>(this List<T> list)
    {
        if (list == null || list.Count == 0) return default;

        var random = new Random();
        return list[random.Next(0, list.Count)];
    }
}