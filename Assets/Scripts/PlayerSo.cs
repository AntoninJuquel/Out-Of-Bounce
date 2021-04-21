using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Systems.Achievement;
using Systems.Save;
using Systems.Unlock;
using Dot;


[CreateAssetMenu(fileName = "New player data", menuName = "Player", order = 0)]
public class PlayerSo : ScriptableObject
{
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vault vault;
    [SerializeField] private List<DotSo> dots;

    private Dictionary<AchievementType, Achievement> _achievements = new Dictionary<AchievementType, Achievement>();
    public Vector3 GetStartPosition() => startPosition;
    public int GetMoney() => vault.GetValue();
    public Vault GetVault() => vault;
    public List<DotSo> GetDots() => dots;

    public void LoadPlayer()
    {
        foreach (var achievementType in AchievementUtilities.AchievementTypesArray())
        {
            _achievements.Add(achievementType, new Achievement {achievementType = achievementType});
        }

        var defaultAchievement = _achievements.Select(achievement => achievement.Value).ToList();
        var loadedAchievements = SaveManager.LoadByBF("achievements.txt", defaultAchievement) as List<Achievement>;

        if (loadedAchievements != null)
            foreach (var achievement in loadedAchievements)
            {
                _achievements[achievement.achievementType] = achievement;
            }

        var save = SaveManager.LoadByBF("dots.txt", dots.Select(dotSo => dotSo.GetStatus()).ToList()) as List<UnlockStatus>;
        for (var i = 0; i < save?.Count; i++)
        {
            dots[i].SetUnlocked(save[i]);
        }

        vault = SaveManager.LoadByBF("vault.txt", vault) as Vault;
    }

    public void SavePlayer()
    {
        SaveManager.SaveByBF("achievements.txt", _achievements.Select(achievement => achievement.Value).ToList());
        SaveManager.SaveByBF("vault.txt", vault);
        SaveManager.SaveByBF("dots.txt", dots.Select(dotsSo => dotsSo.GetStatus()).ToList());
    }

    public void UpdatePlayer(Dictionary<AchievementType, float> achievementValues)
    {
        foreach (var achievementType in AchievementUtilities.AchievementTypesArray())
        {
            _achievements[achievementType].UpdateAchievement(achievementValues[achievementType]);
        }

        vault.AddValue((int) achievementValues[AchievementType.Money]);

        foreach (var dot in dots)
        {
            dot.UpdateStatus(vault);
        }
        
        SavePlayer();
    }
}