using System.Collections;
using Systems.Statistic;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UserInterface
{
    public class CanvasManager : MonoBehaviour
    {
        public static CanvasManager Instance;
        [SerializeField] private PlayerSo playerSo;
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private Slider[] sliders;
        [SerializeField] private Canvas[] canvasArray;
        [SerializeField] private TextMeshProUGUI heightText, scoreText, platformText, versionText;
        [SerializeField] private TextMeshProUGUI endHeightText, endScoreText, endKillsText, endTimerText, endCoinsText, bestScore;

        private const string ScoreFormat = "00000000";
        private const string HeightFormat = "000.00";

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            foreach (var slider in sliders)
            {
                var value = PlayerPrefs.GetFloat(slider.name, .5f);
                slider.value = value;
                audioMixer.SetFloat(slider.name, Mathf.Log10(value) * 20f);
            }

            SetActiveCanvas(canvasArray[0]);
            versionText.text = string.Concat("version : ", Application.version);
        }

        public void SetActiveCanvas(Canvas target)
        {
            foreach (var canvas in canvasArray)
            {
                canvas.gameObject.SetActive(target == canvas);
            }
        }

        public void OpenUrl(string url) => Application.OpenURL(url);

        public void SetVolume(float value)
        {
            if (!EventSystem.current.currentSelectedGameObject) return;
            var sliderName = EventSystem.current.currentSelectedGameObject.name;
            PlayerPrefs.SetFloat(sliderName, value);
            audioMixer.SetFloat(sliderName, Mathf.Log10(value) * 20f);
        }

        public void SetHeightText(float value)
        {
            value = Mathf.Max(0, value);
            heightText.text = string.Concat(value.ToString(HeightFormat), "m");
        }

        public void SetScoreText(float score)
        {
            scoreText.text = score.ToString(ScoreFormat);
        }

        public void SetPlatformText(int value)
        {
            platformText.text = string.Concat(value, "|");
        }

        public void SetupEndScreen()
        {
            var height = playerSo.GetStatistic(StatisticType.Height).last;
            var score = playerSo.GetStatistic(StatisticType.Score).last;
            var kills = playerSo.GetStatistic(StatisticType.Kills).last;
            var coins = playerSo.GetStatistic(StatisticType.Money).last;
            var timer = Utilities.FormatTime(playerSo.GetStatistic(StatisticType.Time).last);

            StartCoroutine(LerpText("SCORE\n", "", score, ScoreFormat, endScoreText));
            StartCoroutine(LerpText("", "m", height, HeightFormat, endHeightText));
            StartCoroutine(LerpText("", "", kills, "0", endKillsText));
            StartCoroutine(LerpText("", " $", coins, "0", endCoinsText));
            
            endTimerText.text = string.Concat(timer[0].ToString("00"), " : ", timer[1].ToString("00"));
            bestScore.text = string.Concat("BEST SCORE : ", playerSo.GetStatistic(StatisticType.Score).best.ToString(ScoreFormat));
        }

        private IEnumerator LerpText(string before, string after, float to, string format, TextMeshProUGUI textMeshProUGUI)
        {
            var elapsedTime = 0f;
            var waitTime = 3f;
            while (elapsedTime < waitTime)
            {
                textMeshProUGUI.text = string.Concat(before, Mathf.Lerp(0, to, elapsedTime / waitTime).ToString(format), after);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            textMeshProUGUI.text = string.Concat(before, to.ToString(format), after);
            yield return null;
        }
    }
}