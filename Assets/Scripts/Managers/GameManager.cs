using System.Collections;
using Systems.Audio;
using Systems.Chunk;
using Ball;
using Controllers;
using Platform;
using Score;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UserInterface;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        [SerializeField] private GameObject lavaPit;
        [SerializeField] private PlayerSo playerSo;
        [SerializeField] private UnityEvent onGameOver;
        [SerializeField] private Button watchAd;
        public static GameStatus GameStatus;
        private static GameStatus _gameStatusBeforePause;
        private float _lavaY, _startTime;
        private Vector3 _deathPosition;

        private void Awake()
        {
            Instance = this;
            Application.targetFrameRate = 144;
            _lavaY = lavaPit.transform.position.y;
            playerSo.LoadPlayer();
            AudioManager.Instance.Play("theme", 0);
        }

        private IEnumerator StartRoutine(bool reset)
        {
            GameStatus = GameStatus.Starting;
            yield return new WaitUntil(() => SceneManager.GetSceneByName("GameScene").isLoaded);
            ChunkManager.Instance.StartChunk(reset ? playerSo.GetStartPosition() : _deathPosition);
            BallManager.Instance.SpawnBall(reset ? playerSo.GetStartPosition() : _deathPosition + Vector3.up * 20, out var firstBallRb);
            CameraController.Instance.SetTarget(firstBallRb);
            if (reset)
            {
                ScoreManager.Instance.ResetScores();
                CanvasManager.Instance.SetScoreText(0);
            }

            PlatformManager.Instance.ResupplyPlatforms(1);
            watchAd.interactable = reset;
            yield return new WaitUntil(() => Input.GetMouseButtonUp(0) && GameStatus != GameStatus.Paused);
            AudioManager.Instance.Stop("theme");
            AudioManager.Instance.Play("theme", 1);
            GameStatus = GameStatus.Playing;
            firstBallRb.simulated = true;
            _startTime = Time.time;
        }
        
        private IEnumerator EndRoutine()
        {
            yield return new WaitForEndOfFrame();
            ScoreManager.Instance.UpdatePlayerSo();
            CanvasManager.Instance.SetupEndScreen();
        }

        public void StartGame()
        {
            Time.timeScale = 1;
            lavaPit.SetActive(true);
            var load = SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Additive);
            load.completed += asyncOperation => StartCoroutine(StartRoutine(true));
        }

        public void RestartGame()
        {
            var load = SceneManager.UnloadSceneAsync("GameScene");
            load.completed += asyncOperation => StartGame();
        }

        public void Respawn()
        {
            StartCoroutine(StartRoutine(false));
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

        public void GameOver(Vector2 position)
        {
            GameStatus = GameStatus.GameOver;
            _deathPosition = position;
            onGameOver?.Invoke();
            AudioManager.Instance.Stop("theme");
            AudioManager.Instance.Play("theme", 0);
            ScoreManager.Instance.UpdateTime(Time.time - _startTime);
            StartCoroutine(EndRoutine());
        }
        
        public void MainMenu()
        {
            lavaPit.SetActive(false);
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