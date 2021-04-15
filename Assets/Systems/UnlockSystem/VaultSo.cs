using UnityEngine;

namespace Systems.UnlockSystem
{
    public class VaultSo : ScriptableObject
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