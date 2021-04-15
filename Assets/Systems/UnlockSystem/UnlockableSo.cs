using UnityEngine;

namespace Systems.UnlockSystem
{
    public class UnlockableSo : ScriptableObject
    {
        [SerializeField] private int value;
        [SerializeField] private int levelRequired;
        private UnlockStatus _status = UnlockStatus.Locked;

        private enum UnlockStatus
        {
            Locked,
            Unlockable,
            Unlocked
        }

        public void UpdateStatus(VaultSo vaultSo)
        {
            if (_status != UnlockStatus.Locked) return;
            if (vaultSo.GetLevel() >= levelRequired)
                _status = UnlockStatus.Unlockable;
        }

        public void Unlock(VaultSo vaultSo)
        {
            if (_status != UnlockStatus.Unlockable) return;
            var res = vaultSo.GetValue() - value;
            if (res <= 0) return;
            _status = UnlockStatus.Unlocked;
            vaultSo.SetValue(res);
        }
    }
}