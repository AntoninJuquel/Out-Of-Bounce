using System.Collections;
using Systems.Chunk;
using Ball;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameObject lavaPit;
    [SerializeField] private PlayerSo playerSo;
    [SerializeField] private UnityEvent onGameOver;
    public static GameStatus GameStatus;
    private static GameStatus _gameStatusBeforePause;
    private float _lavaY, _startTime;

    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 144;
        _lavaY = lavaPit.transform.position.y;
        playerSo.LoadPlayer();
    }

    private IEnumerator StartRoutine()
    {
        GameStatus = GameStatus.Starting;
        yield return new WaitUntil(() => SceneManager.GetSceneByName("GameScene").isLoaded);
        ChunkManager.Instance.StartChunk(playerSo.GetStartPosition());
        BallManager.Instance.SpawnBall(playerSo.GetStartPosition(), out var firstBallRb);
        CameraController.Instance.SetTarget(firstBallRb);
        yield return new WaitUntil(() => Input.GetMouseButtonUp(0) && GameStatus != GameStatus.Paused);
        GameStatus = GameStatus.Playing;
        firstBallRb.simulated = true;
        _startTime = Time.time;
        ScoreManager.Instance.ResetScores();
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        var load = SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Additive);
        load.completed += asyncOperation => StartCoroutine(StartRoutine());
    }

    public void RestartGame()
    {
        var load = SceneManager.UnloadSceneAsync("GameScene");
        load.completed += asyncOperation => StartGame();
    }

    public void TogglePause()
    {
        if (GameStatus == GameStatus.Paused)
        {
            GameStatus = _gameStatusBeforePause;
            Time.timeScale = 1;
        }
        else
        {
            _gameStatusBeforePause = GameStatus;
            GameStatus = GameStatus.Paused;
            Time.timeScale = 0;
        }
    }

    public void GameOver()
    {
        GameStatus = GameStatus.GameOver;
        onGameOver?.Invoke();

        ScoreManager.Instance.UpdateTime(Time.time - _startTime);
        ScoreManager.Instance.UpdatePlayerSo();
        playerSo.SavePlayer();
    }

    public void MainMenu()
    {
        StopAllCoroutines();
        Time.timeScale = 1;
        SceneManager.UnloadSceneAsync("GameScene");
    }

    public void SetLavaPosition(float xPosition)
    {
        lavaPit.transform.position = new Vector3(xPosition, _lavaY);
    }
}

public enum GameStatus
{
    Starting,
    Playing,
    Paused,
    GameOver
}