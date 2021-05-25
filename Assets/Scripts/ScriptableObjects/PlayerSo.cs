using System.Collections.Generic;
using System.Linq;
using Systems.Save;
using Systems.Statistic;
using Systems.Unlock;
using Dot;
using Skin;
using UnityEngine;
using Upgrade;
using Upgrade.UpgradeSos;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New player data", menuName = "Player", order = 0)]
    public class PlayerSo : ScriptableObject
    {
        [SerializeField] private Vault vault;
        [SerializeField] private List<DotSo> dots;
        [SerializeField] private List<UpgradeSo> upgrades;
        [SerializeField] private List<SkinSetSo> skinSets;

        private readonly Dictionary<StatisticType, Statistic> _statistics = new Dictionary<StatisticType, Statistic>();
        public Vector3 GetStartPosition() => Vector3.up * ((upgrades.Find(upgrade => upgrade.GetType() == typeof(HeightUpgrade)).GetLevel() + 1) * 32);
        public int GetPlatformCount() => 3 + upgrades.Find(upgrade => upgrade.GetType() == typeof(PlatformCountUpgrade)).GetLevel();
        public int GetMoney() => vault.GetValue();
        public Vault GetVault() => vault;
        public List<DotSo> GetDots() => dots;
        public List<UpgradeSo> GetUpgrades() => upgrades;
        public List<SkinSetSo> GetSkinSets() => skinSets;
        public Statistic GetStatistic(StatisticType statisticType) => _statistics[statisticType];
        public Dictionary<StatisticType, Statistic> GetStatistics() => _statistics;

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

            var dotsSave = SaveManager.LoadByBF("dots.txt", dots.Select(dotsSo => new UpgradableSave {name = dotsSo.name, level = dotsSo.GetLevel(), unlockStatus = dotsSo.GetStatus()}).ToList()) as List<UpgradableSave>;
            for (var i = 0; i < dotsSave?.Count; i++)
            {
                dots[i].SetUnlocked(dotsSave[i].unlockStatus);
                dots[i].SetLevel(dotsSave[i].level);
            }

            var upgradesSave = SaveManager.LoadByBF("upgrades.txt", upgrades.Select(upgradeSo => new UpgradableSave {name = upgradeSo.name, level = upgradeSo.GetLevel(), unlockStatus = upgradeSo.GetStatus()}).ToList()) as List<UpgradableSave>;
            for (var i = 0; i < upgradesSave?.Count; i++)
            {
                upgrades[i].SetUnlocked(upgradesSave[i].unlockStatus);
                upgrades[i].SetLevel(upgradesSave[i].level);
            }

            vault = SaveManager.LoadByBF("vault.txt", vault) as Vault;

            var skins = new List<SkinSo>();
            foreach (var skinSet in skinSets)
            {
                skins.AddRange(skinSet.GetSkins());
            }
            var skinsSave = SaveManager.LoadByXML("skins.txt", skins.Select(skinSo => new SkinSave {name = skinSo.name, level = skinSo.GetLevel(), unlockStatus = skinSo.GetStatus(), Selected = skinSo.Selected()}).ToList()) as List<SkinSave>;
            foreach (var skinSet in skinSets)
            {
                foreach (var skinSo in skinSet.GetSkins())
                {
                    var skin = skinsSave?.FirstOrDefault(s => s.name == skinSo.name);
                    if (skin == null) continue;
                    skinSo.SetUnlocked(skin.unlockStatus);
                    skinSo.SetLevel(skin.level);
                    skinSo.SetSelected(skin.Selected);
                }
            }
        }

        public void SavePlayer()
        {
            SaveManager.SaveByBF("statistics.txt", _statistics.Select(statistic => statistic.Value).ToList());
            SaveManager.SaveByBF("vault.txt", vault);
            SaveManager.SaveByBF("dots.txt", dots.Select(dotSo => new UpgradableSave {name = dotSo.name, level = dotSo.GetLevel(), unlockStatus = dotSo.GetStatus()}).ToList());
            SaveManager.SaveByBF("upgrades.txt", upgrades.Select(upgradeSo => new UpgradableSave {name = upgradeSo.name, level = upgradeSo.GetLevel(), unlockStatus = upgradeSo.GetStatus()}).ToList());
            var skins = new List<SkinSo>();
            foreach (var skinSet in skinSets)
            {
                skins.AddRange(skinSet.GetSkins());
            }

            SaveManager.SaveByXML("skins.txt", skins.Select(skinSo => new SkinSave {name = skinSo.name, level = skinSo.GetLevel(), unlockStatus = skinSo.GetStatus(), Selected = skinSo.Selected()}).ToList());
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
            vault.deltalevel = vault.level - oldLevel;
            vault.AddValue(10 * vault.deltalevel);

            foreach (var dot in dots)
            {
                dot.UpdateStatus(vault);
            }

            foreach (var upgrade in upgrades)
            {
                upgrade.UpdateStatus(vault);
            }

            SavePlayer();
        }

        private Dictionary<SkinType, List<SkinSo>> _unlockedSkins = new Dictionary<SkinType, List<SkinSo>>();

        public void CacheSkins()
        {
            _unlockedSkins = new Dictionary<SkinType, List<SkinSo>>();

            foreach (var skinType in SkinUtilities.SkinTypesArray())
            {
                var skins = new List<SkinSo>();
                foreach (var skinSet in skinSets)
                {
                    skins = skinSet.GetSkins().FindAll(s => s.Unlocked() && s.GetSkinType() == skinType && s.Selected());
                }

                _unlockedSkins.Add(skinType, skins);
            }
        }

        public Dictionary<SkinType, List<SkinSo>> GetUnlockedSkins() => _unlockedSkins;
    }
}