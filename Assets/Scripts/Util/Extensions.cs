using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
}