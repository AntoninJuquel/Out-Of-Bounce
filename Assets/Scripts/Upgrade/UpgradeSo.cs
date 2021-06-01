using Systems.Unlock;
using UnityEngine;

namespace Upgrade
{
    public abstract class UpgradeSo : UnlockableSo
    {
        public UpgradeType type;
        public virtual void OnEnableUpgrade(GameObject gameObject){}
        public virtual void UpdateUpgrade(GameObject gameObject){}
        public virtual void FixedUpdateUpgrade(GameObject gameObject){}
        public virtual void OnDisableUpgrade(GameObject gameObject){}
        public virtual void OnBounceUpgrade(GameObject gameObject, GameObject other){}
    }

    public enum UpgradeType
    {
        Ball,
        Platform
    }
}