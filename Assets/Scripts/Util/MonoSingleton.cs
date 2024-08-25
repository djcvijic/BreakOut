using UnityEditor;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    // ReSharper disable once StaticMemberInGenericType
    private static readonly object Lock = new();

    // ReSharper disable once StaticMemberInGenericType
    private static bool applicationIsQuitting;

    private static T instance;

    public static T Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                return null;
            }

            lock (Lock)
            {
                if (instance == null)
                {
                    instance = GetOrCreateInstance();
                    if (instance != null) instance.ApplyPersistenceIfNeeded();
                }

                return instance;
            }
        }
    }

    private static T GetOrCreateInstance()
    {
        var instances = FindObjectsOfType<T>();
        switch (instances.Length)
        {
            case 0:
                return null;
            case 1:
                return instances[0];
            default:
            {
                for (var i = 1; i < instances.Length; i++)
                {
                    Destroy(instances[i].gameObject);
                }

                return instances[0];
            }
        }
    }

    private void ApplyPersistenceIfNeeded()
    {
        if (!typeof(IPersistentMonoSingleton).IsAssignableFrom(typeof(T))) return;
#if UNITY_EDITOR
        if (transform.parent != null)
        {
            Debug.LogWarning("MonoSingleton must be a root game object so DontDestroyOnLoad can be applied!");
            Selection.activeGameObject = gameObject;
            Debug.Break();
            return;
        }
#endif
        DontDestroyOnLoad(gameObject);
    }


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        ApplyPersistenceIfNeeded();
        OnAwake();
    }

    protected virtual void OnAwake()
    {
    }

    private void OnApplicationQuit()
    {
        applicationIsQuitting = true;
    }
}