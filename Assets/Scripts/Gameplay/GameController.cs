using Messages;
using UnityEngine;
using Util;

public class GameController : MonoSingleton<GameController>
{
    public enum State
    {
        Playing,
        GameOver
    }

    public State CurrentState { get; private set; } = State.Playing;

    private void OnEnable()
    {
        Notifier.Instance.Subscribe<BallDestroyedMessage>(OnBallDestroyed);
    }

    private void OnDisable()
    {
        Notifier.Instance.Unsubscribe<BallDestroyedMessage>(OnBallDestroyed);
    }

    private void OnBallDestroyed(BallDestroyedMessage message)
    {
        CheckGameOver();
    }

    private void CheckGameOver()
    {
        var activeBalls = FindObjectsByType<Ball>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        if (activeBalls.Length == 0)
        {
            CurrentState = State.GameOver;
        }
    }
}