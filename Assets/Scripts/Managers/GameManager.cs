using System.Collections;
using Systems.Achievement;
using Systems.Audio;
using Systems.Chunk;
using Ball;
using Controllers;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UserInterface;

namespace Managers
{
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
            AudioManager.Instance.Play("theme",0);
        }

        private IEnumerator StartRoutine()
        {
            GameStatus = GameStatus.Starting;
            CanvasManager.Instance.SetScoreText(0);
            yield return new WaitUntil(() => SceneManager.GetSceneByName("GameScene").isLoaded);
            ChunkManager.Instance.StartChunk(playerSo.GetStartPosition());
            BallManager.Instance.SpawnBall(playerSo.GetStartPosition(), out var firstBallRb);
            CameraController.Instance.SetTarget(firstBallRb);
            yield return new WaitUntil(() => Input.GetMouseButtonUp(0) && GameStatus != GameStatus.Paused);
            AudioManager.Instance.Stop("theme");
            AudioManager.Instance.Play("theme",1);
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
                CameraController.Instance.StopShake();
                Time.timeScale = 0;
            }
        }

        public void GameOver()
        {
            GameStatus = GameStatus.GameOver;
            onGameOver?.Invoke();
            AudioManager.Instance.Stop("theme");
            AudioManager.Instance.Play("theme",0);
            ScoreManager.Instance.UpdateTime(Time.time - _startTime);
            ScoreManager.Instance.UpdatePlayerSo();
            CanvasManager.Instance.SetupEndScreen();
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
}