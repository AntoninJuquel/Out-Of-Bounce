using System.Collections.Generic;
using System.Linq;
using Systems.Unlock;
using UnityEngine;

namespace Skin
{
    [CreateAssetMenu(fileName = "New skin set", menuName = "Skins/Set", order = 0)]
    public class SkinSetSo : UnlockableSo
    {
        [SerializeField] private List<SkinSo> skins;

        public new bool Unlocked()
        {
            status = skins.All(s => s.Unlocked()) ? UnlockStatus.Unlocked : UnlockStatus.Unlockable;
            return status == UnlockStatus.Unlocked;
        }
        public List<SkinSo> GetSkins() => skins;
    }
}