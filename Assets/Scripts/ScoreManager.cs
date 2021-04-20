using System;
using System.Collections;
using System.Collections.Generic;
using Systems.Achievement;
using UnityEngine;
using UserInterface;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    [SerializeField] private PlayerSo playerSo;
    [SerializeField] private List<AchievementValue> achievementValues;

    private float _lerptScore;

    private void Awake()
    {
        Instance = this;
        foreach (var achievementType in (AchievementType[]) Enum.GetValues(typeof(AchievementType)))
        {
            var achievement = new AchievementValue {achievementType = achievementType};
            achievementValues.Add(achievement);
        }
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
        var score = achievementValues.Find(achievement => achievement.achievementType == AchievementType.Score);
        score.value += (value * 1000);
        StopCoroutine(nameof(LerpScore));
        StartCoroutine(LerpScore(_lerptScore, score.value));
    }

    public void UpdateHeight(float value)
    {
        var height = achievementValues.Find(achievement => achievement.achievementType == AchievementType.Height);
        if (value <= height.value) return;
        height.value = value;
    }

    public void UpdateKills() => achievementValues.Find(achievement => achievement.achievementType == AchievementType.Kills).value += 1;

    public void UpdateTime(float value)
    {
        achievementValues.Find(achievement => achievement.achievementType == AchievementType.Time).value = value;
    }

    public void UpdateMoney(float value)
    {
        var money = achievementValues.Find(achievement => achievement.achievementType == AchievementType.Money);
        money.value += value;
    }

    public void UpdatePlayerSo()
    {
        playerSo.UpdateAchievements(achievementValues);
    }

    public void ResetScores()
    {
        foreach (var achievementValue in achievementValues)
        {
            achievementValue.value = 0;
        }
    }
}