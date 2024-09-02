using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Messages;
using UnityEngine;
using Util;

public class GameController : MonoSingleton<GameController>
{
    [SerializeField] private int maxLives = 3;
    [SerializeField] private LevelGridView levelGrid;

    public enum State
    {
        LevelStarting,
        Playing,
        Paused,
        LevelComplete,
        GameOver
    }

    private Paddle _paddle;

    public State CurrentState { get; private set; }
    public int CurrentLives { get; private set; }
    public int CurrentScore { get; private set; }
    public int PowerUpSecondsRemaining { get; private set; }

    public int LevelIndex
    {
        get => Meta.LevelIndex;
        private set => Meta.LevelIndex = value;
    }

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

    private async void StartLevel()
    {
        var level = LevelLoader.Load(LevelIndex);
        levelGrid.Initialize(level.Grid);
        Attach();
        CurrentState = State.LevelStarting;

        await Task.Delay(2000);

        CurrentState = State.Playing;
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
        CheckLevelComplete();
    }

    private void CheckLevelComplete()
    {
        var remainingBricks = FindObjectsByType<Brick>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        if (remainingBricks.Any(x => !x.IsInvincible)) return;

        CurrentState = State.LevelComplete;
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

    public void NextLevel()
    {
        LevelIndex += 1;
        StartLevel();
    }
}