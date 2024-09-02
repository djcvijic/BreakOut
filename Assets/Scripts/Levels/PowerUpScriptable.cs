using System.Collections;
using UnityEngine;

public abstract class PowerUpScriptable : ScriptableObject
{
    [field: SerializeField] public int DurationSeconds { get; private set; } = 10;

    private Coroutine _coroutine;

    public int SecondsRemaining { get; private set; }

    public abstract bool HasDuration { get; }

    public static PowerUpScriptable Load(string assetName)
    {
        return Resources.Load<PowerUpScriptable>($"PowerUps/{assetName}");
    }

    public void Start()
    {
        Run();
        if (!HasDuration) return;

        SecondsRemaining = DurationSeconds;
        _coroutine = CoroutineRunner.Instance.StartCoroutine(AwaitEnd());
    }

    protected abstract void Run();

    private IEnumerator AwaitEnd()
    {
        while (SecondsRemaining > 0)
        {
            yield return new WaitForSeconds(1f);
            SecondsRemaining -= 1;
        }

        End();
    }

    public void Stop()
    {
        if (_coroutine != null) {CoroutineRunner.Instance.StopCoroutine(_coroutine);}

        End();
    }

    private void End()
    {
        _coroutine = null;
        OnEnded();
        Destroy(this);
    }

    protected abstract void OnEnded();
}