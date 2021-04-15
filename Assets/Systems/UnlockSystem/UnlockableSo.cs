using System;
using UnityEngine;

namespace Systems.UnlockSystem
{
    public class UnlockableSo : ScriptableObject
    {
        [SerializeField] private int price;
        [SerializeField] private int levelRequired;
        private UnlockStatus _status = UnlockStatus.Locked;

        private enum UnlockStatus
        {
            Locked,
            Unlockable,
            Unlocked
        }

        public void UpdateStatus(Vault vault)
        {
            if (_status != UnlockStatus.Locked) return;
            if (vault.GetLevel() >= levelRequired)
                _status = UnlockStatus.Unlockable;
        }

        public void Unlock(Vault vault)
        {
            if (_status != UnlockStatus.Unlockable) return;
            var res = vault.GetValue() - price;
            if (res <= 0) return;
            _status = UnlockStatus.Unlocked;
            vault.SetValue(res);
        }
    }

    [Serializable]
    public class Vault
    {
        [SerializeField] private int value;
        [SerializeField] private int level;

        public int GetValue() => value;
        public int GetLevel() => level;

        public void SetValue(int newValue) => value = newValue;
        public void SetLevel(int newLevel) => level = newLevel;

        public void AddValue(int amount) => value += amount;
        public void AddLevel(int amount) => level += amount;
    }
}