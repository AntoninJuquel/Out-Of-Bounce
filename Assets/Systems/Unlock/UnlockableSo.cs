using System;
using UnityEngine;

namespace Systems.Unlock
{
    public abstract class UnlockableSo : ScriptableObject
    {
        [Header("Unlock data")] [SerializeField]
        private Sprite[] sprites;

        [SerializeField] protected Color color = Color.white;
        [SerializeField] private int levelRequired;
        [SerializeField] private UnlockStatus status = UnlockStatus.Unlockable;
        [SerializeField] protected int level;
        [SerializeField] private Upgrade[] upgrades;

        public void UpdateStatus(Vault vault)
        {
            if (status != UnlockStatus.Locked) return;
            if (vault.GetLevel() >= levelRequired)
                status = UnlockStatus.Unlockable;
        }

        public void Unlock(Vault vault)
        {
            if (status != UnlockStatus.Unlockable) return;
            var res = vault.GetValue() - upgrades[0].GetPrice();
            if (res < 0) return;
            status = UnlockStatus.Unlocked;
            vault.SetValue(res);
        }

        public void Upgrade(Vault vault)
        {
            var res = vault.GetValue() - upgrades[level + 1].GetPrice();
            var upgradable = res >= 0;
            if (!upgradable) return;
            level = Mathf.Min(level + 1, upgrades.Length - 1);
            vault.SetValue(res);
        }

        public void Downgrade()
        {
            level = Mathf.Max(level - 1, 0);
        }

        public Sprite[] GetSprites() => sprites;
        public bool Unlocked() => status == UnlockStatus.Unlocked;
        public int GetPrice() => GetPrice(level);
        public int GetPrice(int index) => upgrades[index].GetPrice();
        public Color GetColor() => color;
        public string GetDescription() => GetDescription(level);
        public string GetDescription(int index) => upgrades[index].GetDescription();
        public UnlockStatus GetStatus() => status;
        public void SetUnlocked(UnlockStatus unlockStatus) => status = unlockStatus;
        public Upgrade[] GetUpgrades() => upgrades;
        public int GetLevel() => level;
        public void SetLevel(int lvl) => level = lvl;
        public bool MaxLevel() => level == upgrades.Length - 1;
    }

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
            
            if(delta < 0) return;
            level++;
            experience = 0;
            if(delta > 0) LevelUp(delta);
        }
    }

    [Serializable]
    public class Upgrade
    {
        [SerializeField] private int price;
        [SerializeField] [TextArea] private string description;

        public int GetPrice() => price;
        public string GetDescription() => description;
    }

    [Serializable]
    public class UpgradableSave
    {
        public UnlockStatus unlockStatus;
        public int level;
    }
}