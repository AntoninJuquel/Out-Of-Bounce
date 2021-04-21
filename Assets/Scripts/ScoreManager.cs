using System.Collections;
using System.Collections.Generic;
using Systems.Achievement;
using UnityEngine;
using UserInterface;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    [SerializeField] private PlayerSo playerSo;
    private Dictionary<AchievementType, float> _achievementValues = new Dictionary<AchievementType, float>();

    private float _lerptScore;

    private void Awake()
    {
        Instance = this;
        ResetScores();
        CanvasManager.Instance.SetScoreText(0);
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
        _achievementValues[AchievementType.Score] += (value * 1000);
        StopCoroutine(nameof(LerpScore));
        StartCoroutine(LerpScore(_lerptScore, _achievementValues[AchievementType.Score]));
    }

    public void UpdateHeight(float value)
    {
        var height = _achievementValues[AchievementType.Height];
        if (value <= height) return;
        _achievementValues[AchievementType.Height] = value;
    }

    public void UpdateKills() => _achievementValues[AchievementType.Kills] += 1;

    public void UpdateTime(float value)
    {
        _achievementValues[AchievementType.Time] = value;
    }

    public void UpdateMoney(float value)
    {
        _achievementValues[AchievementType.Money] += value;
    }

    public void UpdatePlayerSo()
    {
        playerSo.UpdateAchievements(_achievementValues);
    }

    public void ResetScores()
    {
        _achievementValues = new Dictionary<AchievementType, float>();
        foreach (var achievementType in AchievementUtilities.AchievementTypesArray())
        {
            _achievementValues.Add(achievementType, 0);
        }
    }
}