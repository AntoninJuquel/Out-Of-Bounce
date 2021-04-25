using System.Collections.Generic;
using System.Linq;
using Systems.Save;
using Systems.Statistic;
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
        public int deltalevel = 0;

        private readonly Dictionary<StatisticType, Statistic> _statistics = new Dictionary<StatisticType, Statistic>();
        public Vector3 GetStartPosition() => startPosition;
        public int GetMoney() => vault.GetValue();
        public Vault GetVault() => vault;
        public List<DotSo> GetDots() => dots;
        public List<SkinSo> GetPlatformSkins() => platformSkins;
        public Statistic GetStatistic(StatisticType statisticType) => _statistics[statisticType];

        public void LoadPlayer()
        {
            foreach (var achievementType in StatisticUtilities.StatisticTypesArray())
            {
                _statistics.Add(achievementType, new Statistic {statisticType = achievementType});
            }

            var defaultAchievement = _statistics.Select(statistic => statistic.Value).ToList();

            if (SaveManager.LoadByBF("statistics.txt", defaultAchievement) is List<Statistic> loadedAchievements)
                foreach (var achievement in loadedAchievements)
                {
                    _statistics[achievement.statisticType] = achievement;
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
            SaveManager.SaveByBF("statistics.txt", _statistics.Select(statistic => statistic.Value).ToList());
            SaveManager.SaveByBF("vault.txt", vault);
            SaveManager.SaveByBF("dots.txt", dots.Select(dotsSo => new UpgradableSave {level = dotsSo.GetLevel(), unlockStatus = dotsSo.GetStatus()}).ToList());
        }

        public void UpdatePlayer(Dictionary<StatisticType, float> achievementValues)
        {
            foreach (var achievementType in StatisticUtilities.StatisticTypesArray())
            {
                _statistics[achievementType].UpdateStatistic(achievementValues[achievementType]);
            }

            vault.AddValue((int) achievementValues[StatisticType.Money]);
            var oldLevel = vault.level;
            vault.LevelUp(achievementValues[StatisticType.Score] / 10000f + (achievementValues[StatisticType.Money] + achievementValues[StatisticType.Kills]) / 10f + achievementValues[StatisticType.Height] / 100f);
            deltalevel = vault.level - oldLevel;
            vault.AddValue(10 * deltalevel);

            foreach (var dot in dots)
            {
                dot.UpdateStatus(vault);
            }

            SavePlayer();
        }
    }
}