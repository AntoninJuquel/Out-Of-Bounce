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
        public virtual void OnCollisionEnter2DUpgrade(GameObject gameObject, Collision2D other){}
        public virtual void OnTriggerEnter2DUpgrade(GameObject gameObject, Collider2D other){}
        public virtual void OnDisableUpgrade(GameObject gameObject){}
        public virtual void OnBounceUpgrade(GameObject gameObject, GameObject other){}
        public virtual void TriggerUpgrade(GameObject gameObject){}
    }

    public enum UpgradeType
    {
        Ball,
        Platform,
        Settings
    }
}