using Messages;
using UnityEngine;
using Util;

public class GameController : MonoSingleton<GameController>
{
    [SerializeField] private int maxLives = 3;
    [SerializeField] private LevelGridView levelGrid;

    public enum State
    {
        Playing,
        Paused,
        GameOver
    }

    private Paddle _paddle;

    public State CurrentState { get; private set; } = State.Playing;
    public int CurrentLives { get; private set; }
    public int CurrentScore { get; private set; }
    public int PowerUpSecondsRemaining { get; private set; }

    public int LevelIndex => Meta.LevelIndex;

    private void OnEnable()
    {
        Notifier.Instance.Subscribe<BallDestroyedMessage>(OnBallDestroyed);
        Notifier.Instance.Subscribe<BrickDestroyedMessage>(OnBrickDestroyed);
    }

    private void OnDisable()
    {
        Notifier.Instance.Unsubscribe<BallDestroyedMessage>(OnBallDestroyed);
        Notifier.Instance.Unsubscribe<BrickDestroyedMessage>(OnBrickDestroyed);
    }

    protected override void OnAwake()
    {
        base.OnAwake();

        _paddle = FindAnyObjectByType<Paddle>(FindObjectsInactive.Exclude);
        StartGame();
    }

    private void StartGame()
    {
        CurrentLives = maxLives;
        CurrentScore = 0;
        StartLevel();
    }

    private void StartLevel()
    {
        var level = LevelLoader.Load(LevelIndex);
        levelGrid.Initialize(level.Grid);
        Attach();
    }

    private void Attach()
    {
        var ball = FindAnyObjectByType<Ball>(FindObjectsInactive.Include);
        ball.gameObject.SetActive(true);
        _paddle.Attach(ball);
    }

    private void OnBrickDestroyed(BrickDestroyedMessage message)
    {
        CurrentScore += message.scoreContribution;
    }

    private void OnBallDestroyed(BallDestroyedMessage message)
    {
        CheckLoseLife();
    }

    private void CheckLoseLife()
    {
        var anyBall = FindAnyObjectByType<Ball>(FindObjectsInactive.Exclude);
        if (anyBall != null) return;

        CurrentLives -= 1;
        GameOverOrReattach();
    }

    private void GameOverOrReattach()
    {
        if (CurrentLives <= 0)
        {
            CurrentState = State.GameOver;
        }
        else
        {
            Attach();
        }
    }

    public void Pause()
    {
        if (CurrentState != State.Playing) return;

        CurrentState = State.Paused;
    }

    public void UnPause()
    {
        if (CurrentState != State.Paused) return;

        CurrentState = State.Playing;
    }
}