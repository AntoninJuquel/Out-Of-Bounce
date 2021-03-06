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

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        [SerializeField] private GameObject lavaPit, tutorial;
        [SerializeField] private PlayerSo playerSo;
        [SerializeField] private UnityEvent onGameOver;
        [SerializeField] private Button watchAd;
        [SerializeField] private Button mainMenuButton, pauseButton, resumeButton;
        public static GameStatus GameStatus;
        private static GameStatus gameStatusBeforePause;
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

        private void Update()
        {
            if (GameStatus != GameStatus.Paused)
            {
                Time.timeScale += (1f / 3f) * Time.unscaledDeltaTime;
                Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
            }
#if !UNITY_ANDROID && !UNITY_IOS
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                switch (GameStatus)
                {
                    case GameStatus.GameOver:
                        mainMenuButton.onClick.Invoke();
                        break;
                    case GameStatus.Paused:
                        resumeButton.onClick.Invoke();
                        break;
                    case GameStatus.Playing:
                        pauseButton.onClick.Invoke();
                        break;
                    case GameStatus.Starting:
                        pauseButton.onClick.Invoke();
                        break;
                }
            }
#endif
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

        // private IEnumerator CheckForUpdate()
        // {
        //     _appUpdateManager = new AppUpdateManager();
        //     var appUpdateInfoOperation = _appUpdateManager.GetAppUpdateInfo();
        //
        //     // Wait until the asynchronous operation completes.
        //     yield return appUpdateInfoOperation;
        //
        //     if (appUpdateInfoOperation.IsSuccessful)
        //     {
        //         var appUpdateInfoResult = appUpdateInfoOperation.GetResult();
        //         if (appUpdateInfoResult.UpdateAvailability == UpdateAvailability.UpdateAvailable)
        //         {
        //             Debug.Log(UpdateAvailability.UpdateAvailable.ToString());
        //             var appUpdateOptions = AppUpdateOptions.ImmediateAppUpdateOptions();
        //             StartCoroutine(StartUpdate(appUpdateInfoResult, appUpdateOptions));
        //         }
        //         else
        //         {
        //             Debug.Log("No Update Available");
        //         }
        //
        //     }
        // }
        //
        // private IEnumerator StartUpdate(AppUpdateInfo appUpdateInfo, AppUpdateOptions appUpdateOptions)
        // {
        //     var startUpdateRequest = _appUpdateManager.StartUpdate(appUpdateInfo,appUpdateOptions);
        //     yield return startUpdateRequest;
        // }

        public void StartGame()
        {
            playerSo.CacheSkins();
            Time.timeScale = 1;
            lavaPit.SetActive(true);
            tutorial.SetActive(true);
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
                GameStatus = gameStatusBeforePause;
                Time.timeScale = 1;
            }
            else
            {
                gameStatusBeforePause = GameStatus;
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
            GameStatus = GameStatus.Menu;
            lavaPit.SetActive(false);
            StopAllCoroutines();
            Time.timeScale = 1;
            SceneManager.UnloadSceneAsync("GameScene");
        }

        public void SetLavaPosition(float xPosition)
        {
            lavaPit.transform.position = new Vector3(xPosition, _lavaY);
        }

        public float GetDeathPosition() => lavaPit.transform.position.y + (lavaPit.transform.localScale.y / 2f);

        public void ShareGame()
        {
#if UNITY_ANDROID || UNITY_IOS
            new NativeShare()
                .SetTitle("Partager Out of Bounce")
                .SetText("Jouez à Out of Bounce et marquez le plus de points possible : ")
                .SetUrl("https://play.google.com/store/apps/details?id=com.kibblecorp.outofbounce")
                .Share();
#else
            GUIUtility.systemCopyBuffer = "https://somindras.itch.io/out-of-bounce";
#endif
        }

        public void QuitGame() => Application.Quit();
    }

    public enum GameStatus
    {
        Menu,
        Starting,
        Playing,
        Paused,
        GameOver
    }
}