using System;
using System.Collections.Generic;
using UnityEngine;
using Systems.Achievement;
using Systems.Save;
using Systems.Unlock;


[CreateAssetMenu(fileName = "New player data", menuName = "Player", order = 0)]
public class PlayerSo : ScriptableObject
{
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vault vault;
    [SerializeField] private List<Achievement> achievements;

    public void UpdateAchievements(IEnumerable<AchievementValue> achievementValues)
    {
        foreach (var achievementValue in achievementValues)
        {
            achievements.Find(achievement => achievement.achievementType == achievementValue.achievementType).UpdateAchievement(achievementValue.value);
        }

        vault.AddValue((int) achievements.Find(achievement => achievement.achievementType == AchievementType.Money).last);
    }

    public Vector3 GetStartPosition() => startPosition;
    public int GetMoney() => vault.GetValue();
    public Vault GetVault() => vault;

    public void LoadPlayer()
    {
        var defaultAchievement = new List<Achievement>();
        var achievementTypes = (AchievementType[]) Enum.GetValues(typeof(AchievementType));
        foreach (var achievementType in achievementTypes)
        {
            defaultAchievement.Add(new Achievement {achievementType = achievementType});
        }

        var loadedAchievements = SaveManager.LoadByXML("achievements.txt", defaultAchievement) as List<Achievement>;

        foreach (var achievementType in achievementTypes)
        {
            if (loadedAchievements.Find(achievement => achievement.achievementType == achievementType) != null) continue;
            loadedAchievements.Add(new Achievement {achievementType = achievementType});
        }

        achievements = loadedAchievements;
        vault = SaveManager.LoadByXML("vault.txt", vault) as Vault;
    }

    public void SavePlayer()
    {
        SaveManager.SaveByXML("achievements.txt", achievements);
        SaveManager.SaveByXML("vault.txt", vault);
    }
}