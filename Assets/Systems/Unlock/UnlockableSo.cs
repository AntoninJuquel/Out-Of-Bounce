using System;
using UnityEngine;

namespace Systems.Unlock
{
    public abstract class UnlockableSo : ScriptableObject
    {
        [Header("Unlock data")] [SerializeField]
        private Sprite[] sprites;

        [SerializeField] protected Color color = Color.white;
        [SerializeField] private int price;
        [SerializeField] private int levelRequired;
        [SerializeField] [TextArea] private string description;
        [SerializeField] private UnlockStatus status = UnlockStatus.Locked;

        public void UpdateStatus(Vault vault)
        {
            if (status != UnlockStatus.Locked) return;
            if (vault.GetLevel() >= levelRequired)
                status = UnlockStatus.Unlockable;
        }

        public void Unlock(Vault vault)
        {
            if (status != UnlockStatus.Unlockable) return;
            var res = vault.GetValue() - price;
            if (res < 0) return;
            status = UnlockStatus.Unlocked;
            vault.SetValue(res);
        }

        public Sprite[] GetSprites() => sprites;
        public bool Unlocked() => status == UnlockStatus.Unlocked;
        public int GetPrice() => price;
        public Color GetColor() => color;
        public string GetDescription() => description;
        public UnlockStatus GetStatus() => status;
        public void SetUnlocked(UnlockStatus unlockStatus) => status = unlockStatus;
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

        public int GetValue() => value;
        public int GetLevel() => level;

        public void SetValue(int newValue) => value = newValue;
        public void SetLevel(int newLevel) => level = newLevel;

        public void AddValue(int amount) => value += amount;
        public void AddLevel(int amount) => level += amount;

        public void LevelUp(int levelGained, float experienceGained)
        {
            experience += experienceGained;
            var calculatedLevel = level + levelGained;
            var experienceRequired = calculatedLevel < 15 ? 2 * calculatedLevel + 7 : (calculatedLevel < 30 ? 5 * calculatedLevel - 38 : 9 * calculatedLevel - 158);
            if (experience >= experienceRequired)
            {
                var delta = experience - experienceRequired;
                experience = delta;
                levelGained++;
                LevelUp(levelGained, 0);
            }
            else level += levelGained;
        }
    }
}