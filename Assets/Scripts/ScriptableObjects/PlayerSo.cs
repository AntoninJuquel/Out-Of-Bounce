using System.Collections.Generic;
using System.Linq;
using Systems.Achievement;
using Systems.Save;
using Systems.Unlock;
using Dot;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New player data", menuName = "Player", order = 0)]
    public class PlayerSo : ScriptableObject
    {
        [SerializeField] private Vector3 startPosition;
        [SerializeField] private Vault vault;
        [SerializeField] private List<DotSo> dots;
        [SerializeField] private List<SkinSo> platformSkins;

        private readonly Dictionary<AchievementType, Achievement> _achievements = new Dictionary<AchievementType, Achievement>();
        public Vector3 GetStartPosition() => startPosition;
        public int GetMoney() => vault.GetValue();
        public Vault GetVault() => vault;
        public List<DotSo> GetDots() => dots;
        public List<SkinSo> GetPlatformSkins() => platformSkins;
        public Achievement GetAchievement(AchievementType achievementType) => _achievements[achievementType];
        
        public void LoadPlayer()
        {
            foreach (var achievementType in AchievementUtilities.AchievementTypesArray())
            {
                _achievements.Add(achievementType, new Achievement {achievementType = achievementType});
            }

            var defaultAchievement = _achievements.Select(achievement => achievement.Value).ToList();

            if (SaveManager.LoadByBF("achievements.txt", defaultAchievement) is List<Achievement> loadedAchievements)
                foreach (var achievement in loadedAchievements)
                {
                    _achievements[achievement.achievementType] = achievement;
                }

            var dotsSave = SaveManager.LoadByBF("dots.txt", dots.Select(dotsSo => new UpgradableSave {level = dotsSo.GetLevel(), unlockStatus = dotsSo.GetStatus()}).ToList()) as List<UpgradableSave>;
            for (var i = 0; i < dotsSave?.Count; i++)
            {
                dots[i].SetUnlocked(dotsSave[i].unlockStatus);
                dots[i].SetLevel(dotsSave[i].level);
            }

            vault = SaveManager.LoadByBF("vault.txt", vault) as Vault;
        }

        public void SavePlayer()
        {
            SaveManager.SaveByBF("achievements.txt", _achievements.Select(achievement => achievement.Value).ToList());
            SaveManager.SaveByBF("vault.txt", vault);
            SaveManager.SaveByBF("dots.txt", dots.Select(dotsSo => new UpgradableSave {level = dotsSo.GetLevel(), unlockStatus = dotsSo.GetStatus()}).ToList());
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
}