using Systems.Unlock;
using UnityEngine;

namespace Upgrade
{
    public abstract class UpgradeSo : UnlockableSo
    {
        public virtual void OnEnableUpgrade(GameObject gameObject){}
        public virtual void UpdateUpgrade(GameObject gameObject){}
        public virtual void OnDisableUpgrade(GameObject gameObject){}
    }
}