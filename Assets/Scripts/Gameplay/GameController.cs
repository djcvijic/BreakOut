using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Messages;
using UnityEngine;
using Util;
using Random = System.Random;

public class GameController : MonoSingleton<GameController>
{
    [SerializeField] private int maxLives = 3;
    [SerializeField] private LevelGridView levelGrid;
    [SerializeField] private PowerUp powerUpPrefab;

    public enum State
    {
        LevelStarting,
        Playing,
        Paused,
        LevelComplete,
        GameOver
    }

    private Paddle _paddle;
    private List<PowerUpScriptable> _allowedPowerUps;
    private PowerUpScriptable _activePowerUp;

    public State CurrentState { get; private set; }
    public int CurrentLives { get; private set; }
    public int CurrentScore { get; private set; }
    public int PowerUpSecondsRemaining => _activePowerUp?.SecondsRemaining ?? 0;

    public int LevelIndex
    {
        get => Meta.LevelIndex;
        private set => Meta.LevelIndex = value;
    }

    public Ball AnyBall => FindAnyObjectByType<Ball>(FindObjectsInactive.Exclude);

    private void OnEnable()
    {
        Notifier.Instance.Subscribe<BallDestroyedMessage>(OnBallDestroyed);
        Notifier.Instance.Subscribe<BrickDestroyedMessage>(OnBrickDestroyed);
        Notifier.Instance.Subscribe<PowerUpCollectedMessage>(OnPowerUpCollected);
    }

    private void OnDisable()
    {
        Notifier.Instance.Unsubscribe<BallDestroyedMessage>(OnBallDestroyed);
        Notifier.Instance.Unsubscribe<BrickDestroyedMessage>(OnBrickDestroyed);
        Notifier.Instance.Unsubscribe<PowerUpCollectedMessage>(OnPowerUpCollected);
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
        _allowedPowerUps = level.AllowedPowerUps;
        Attach();
        StopActivePowerUp();
        ClearAllPowerUps();
        CurrentState = State.LevelStarting;

        await Task.Delay(2000);

        CurrentState = State.Playing;
    }

    private void ClearAllPowerUps()
    {
        var allPowerUps = FindObjectsByType<PowerUp>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var powerUp in allPowerUps)
        {
            Destroy(powerUp.gameObject);
        }
    }

    private void Attach()
    {
        var allBalls = FindObjectsByType<Ball>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        var ball = allBalls[0];
        ball.gameObject.SetActive(true);
        _paddle.Attach(ball);

        // leave the first ball alive, clear the rest
        for (int i = 1; i < allBalls.Length; i++)
        {
            Destroy(allBalls[i].gameObject);
        }
    }

    private void OnBrickDestroyed(BrickDestroyedMessage message)
    {
        CurrentScore += message.scoreContribution;
        CheckDropPowerUp(message.powerUpProbability, message.brickPosition);
        CheckLevelComplete();
    }

    private void CheckDropPowerUp(float powerUpProbability, Vector3 brickPosition)
    {
        if (_allowedPowerUps == null || _allowedPowerUps.Count == 0) return;

        var random = new Random();
        if (powerUpProbability < random.NextDouble()) return;

        var powerUp = _allowedPowerUps.Random();
        if (powerUp == null) return;

        Instantiate(powerUpPrefab, brickPosition, Quaternion.identity)
            .Initialize(powerUp);
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
        if (AnyBall != null) return;

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
            ClearAllPowerUps();
        }
    }

    private void OnPowerUpCollected(PowerUpCollectedMessage message)
    {
        if (!message.scriptable.HasDuration)
        {
            Instantiate(message.scriptable).Start();
            return;
        }

        StopActivePowerUp();

        _activePowerUp = Instantiate(message.scriptable);
        _activePowerUp.Start();
    }

    private void StopActivePowerUp()
    {
        if (_activePowerUp != null)
        {
            _activePowerUp.Stop();
            _activePowerUp = null;
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