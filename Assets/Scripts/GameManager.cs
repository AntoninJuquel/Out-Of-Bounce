using System.Collections;
using Systems.Chunk;
using Ball;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private UnityEvent onGameOver;
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private GameObject lavaPit;
    public static GameStatus GameStatus;
    private static GameStatus _gameStatusBeforePause;
    private float lavaY;

    

    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 144;
        lavaY = lavaPit.transform.position.y;
    }

    private IEnumerator StartRoutine()
    {
        GameStatus = GameStatus.Starting;
        yield return new WaitUntil(() => SceneManager.GetSceneByName("GameScene").isLoaded);
        ChunkManager.Instance.StartChunk(startPosition);
        BallManager.Instance.SpawnBall(startPosition, out var firstBallRb);
        CameraController.Instance.SetTarget(firstBallRb);
        yield return new WaitUntil(() => Input.GetMouseButtonUp(0) && GameStatus != GameStatus.Paused);
        GameStatus = GameStatus.Playing;
        firstBallRb.simulated = true;
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
    }

    public void MainMenu()
    {
        StopAllCoroutines();
        Time.timeScale = 1;
        SceneManager.UnloadSceneAsync("GameScene");
    }

    public void SetLavaPosition(float xPosition)
    {
        lavaPit.transform.position = new Vector3(xPosition, lavaY);
    }
}

public enum GameStatus
{
    Starting,
    Playing,
    Paused,
    GameOver
}