using System;
using System.Collections.Generic;
using Save;
using Sirenix.OdinInspector;
using Systems.Statistic;
using Systems.Unlock;
using Skin;

namespace Legacy
{
    public class LegacySave : SerializedMonoBehaviour
    {
        private void Start()
        {
            LoadLegacyStatistic();
            LoadLegacyDots();
            LoadLegacyUpgrades();
            LoadLegacyVault();
            LoadLegacySkins();
        }

        public Dictionary<StatisticType, Statistic> _statistics;

        private void LoadLegacyStatistic()
        {
            _statistics = new Dictionary<StatisticType, Statistic>();

            if (SaveManager.LoadByBf("statistics.txt", new List<Statistic>()) is List<Statistic> loadedAchievements)
            {
                foreach (var achievement in loadedAchievements)
                {
                    _statistics[achievement.statisticType] = achievement;
                }
            }
        }

        public List<UpgradableSave> dotsSave;

        private void LoadLegacyDots()
        {
            dotsSave = SaveManager.LoadByBf("dots.txt", new List<UpgradableSave>()) as
                List<UpgradableSave>;
        }

        public List<UpgradableSave> upgradesSave;

        private void LoadLegacyUpgrades()
        {
            upgradesSave = SaveManager.LoadByBf("upgrades.txt", new List<UpgradableSave>()) as List<UpgradableSave>;
        }

        public Vault vault;

        private void LoadLegacyVault()
        {
            vault = SaveManager.LoadByBf("vault.txt", new Vault()) as Vault;
        }

        public List<SkinSave> skinsSave;

        private void LoadLegacySkins()
        {
            skinsSave = SaveManager.LoadByBf("skins_bf.txt", new List<SkinSave>()) as List<SkinSave>;
        }
    }
}

namespace Systems.Statistic
{
    [Serializable]
    public class Statistic
    {
        public StatisticType statisticType;
        public float last;
        public float best;
        public float total;

        public void UpdateStatistic(float lastValue)
        {
            last = lastValue;
            total += lastValue;
            if (lastValue <= best) return;
            best = lastValue;
        }
    }

    [Serializable]
    public enum StatisticType
    {
        Score,
        Height,
        Time,
        Kills,
        Money
    }
}

namespace Systems.Unlock
{
    public enum UnlockStatus
    {
        Locked,
        Unlockable,
        Unlocked
    }

    [Serializable]
    public class Vault
    {
        public int value;
        public int level;
        public float experience;
        public int deltalevel = 0;
        public float ExperienceRequired => level < 15 ? 2 * level + 7 : (level < 30 ? 5 * level - 38 : 9 * level - 158);

        public int GetValue() => value;
        public int GetLevel() => level;

        public void SetValue(int newValue) => value = newValue;
        public void SetLevel(int newLevel) => level = newLevel;

        public void AddValue(int amount) => value += amount;
        public void AddLevel(int amount) => level += amount;

        public void LevelUp(float experienceGained)
        {
            experience += experienceGained;

            var delta = experience - ExperienceRequired;

            if (delta < 0) return;
            level++;
            experience = 0;
            if (delta > 0) LevelUp(delta);
        }
    }

    [Serializable]
    public class UpgradableSave
    {
        public string name;
        public UnlockStatus unlockStatus;
        public int level;
    }
}


namespace Skin
{
    [Serializable]
    public class SkinSave : UpgradableSave
    {
        public bool Selected;
    }
}