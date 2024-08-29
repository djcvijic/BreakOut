using Messages;
using UnityEngine;
using Util;

public class GameController : MonoSingleton<GameController>
{
    [SerializeField] private int maxLives = 3;

    public enum State
    {
        Playing,
        GameOver
    }

    private Paddle _paddle;
    private int _currentLives;

    public State CurrentState { get; private set; } = State.Playing;

    private void OnEnable()
    {
        Notifier.Instance.Subscribe<BallDestroyedMessage>(OnBallDestroyed);
    }

    private void OnDisable()
    {
        Notifier.Instance.Unsubscribe<BallDestroyedMessage>(OnBallDestroyed);
    }

    protected override void OnAwake()
    {
        base.OnAwake();

        _paddle = FindAnyObjectByType<Paddle>(FindObjectsInactive.Exclude);
        StartGame();
    }

    private void StartGame()
    {
        _currentLives = maxLives;
        Attach();
    }

    private void Attach()
    {
        var ball = FindAnyObjectByType<Ball>(FindObjectsInactive.Include);
        ball.gameObject.SetActive(true);
        _paddle.Attach(ball);
    }

    private void OnBallDestroyed(BallDestroyedMessage message)
    {
        CheckLoseLife();
    }

    private void CheckLoseLife()
    {
        var anyBall = FindAnyObjectByType<Ball>(FindObjectsInactive.Exclude);
        if (anyBall != null) return;

        _currentLives -= 1;
        GameOverOrReattach();
    }

    private void GameOverOrReattach()
    {
        if (_currentLives <= 0)
        {
            CurrentState = State.GameOver;
        }
        else
        {
            Attach();
        }
    }
}