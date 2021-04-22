using System;
using UnityEngine;

namespace Systems.Unlock
{
    public abstract class UpgradableSo : UnlockableSo
    {
        [Header("Upgrade data")] [SerializeField] [Min(1)]
        protected int level = 1;

        [SerializeField] private Upgrade[] upgrades;

        public void Upgrade(Vault vault)
        {
            var res = vault.GetValue() - upgrades[level - 1].GetPrice();
            var upgradable = res >= 0;
            if (!upgradable) return;
            level = Mathf.Min(level + 1, upgrades.Length);
            vault.SetValue(res);
        }

        public void Downgrade()
        {
            level = Mathf.Max(level - 1, 0);
        }

        public Upgrade[] GetUpgrades() => upgrades;
        public int GetLevel() => level;
        public void SetLevel(int lvl) => level = lvl;
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