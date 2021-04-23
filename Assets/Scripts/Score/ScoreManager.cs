using System.Collections;
using System.Collections.Generic;
using Systems.Pool;
using Systems.Statistic;
using ScriptableObjects;
using UnityEngine;
using UserInterface;

namespace Score
{
    public class ScoreManager : ObjectPool
    {
        public static ScoreManager Instance;
        [SerializeField] private PlayerSo playerSo;
        private Dictionary<StatisticType, float> _statisticValues = new Dictionary<StatisticType, float>();

        private float _lerptScore;

        private void Awake()
        {
            Instance = this;
            ResetScores();
        }

        private IEnumerator LerpScore(float oldScore, float newScore)
        {
            var elapsedTime = 0f;
            var waitTime = 3f;
            while (elapsedTime < waitTime)
            {
                _lerptScore = Mathf.Lerp(oldScore, newScore, elapsedTime / waitTime);
                elapsedTime += Time.deltaTime;
                CanvasManager.Instance.SetScoreText(_lerptScore);
                yield return null;
            }

            CanvasManager.Instance.SetScoreText(newScore);
            yield return null;
        }

        public void UpdateScore(int value)
        {
            _statisticValues[StatisticType.Score] += (value * 1000);
            StopCoroutine(nameof(LerpScore));
            StartCoroutine(LerpScore(_lerptScore, _statisticValues[StatisticType.Score]));
        }

        public void SpawnPopup(int value, Vector2 position)
        {
            SpawnFromPool("Popup", position, Quaternion.identity).GetComponent<ScorePopupController>().Setup((value * 1000).ToString());
        }

        public void UpdateHeight(float value)
        {
            var height = _statisticValues[StatisticType.Height];
            if (value <= height) return;
            _statisticValues[StatisticType.Height] = value;
        }

        public void UpdateKills() => _statisticValues[StatisticType.Kills] += 1;

        public void UpdateTime(float value)
        {
            _statisticValues[StatisticType.Time] = value;
        }

        public void UpdateMoney(float value)
        {
            _statisticValues[StatisticType.Money] += value;
        }

        public void UpdatePlayerSo()
        {
            playerSo.UpdatePlayer(_statisticValues);
        }

        public void ResetScores()
        {
            _statisticValues = new Dictionary<StatisticType, float>();
            foreach (var achievementType in StatisticUtilities.StatisticTypesArray())
            {
                _statisticValues.Add(achievementType, 0);
            }
        }
    }
}