using System.Collections.Generic;
using System.Linq;
using Systems.Save;
using UnityEngine;

namespace Systems.Unlock
{
    [CreateAssetMenu(fileName = "New unlockable DB", menuName = "DataBase/Unlockable", order = 0)]
    public class UnlockableDataBaseSo : ScriptableObject
    {
        public List<UnlockableSo> unlockableSos = new List<UnlockableSo>();

        public void SaveUnlockables()
        {
            var save = unlockableSos.Select(unlockableSo => unlockableSo.GetStatus()).ToList();
            SaveManager.SaveByXML(string.Concat(name, ".txt"), save);
        }

        public void LoadUnlockables()
        {
            var defaultSave = unlockableSos.Select(unlockableSo => unlockableSo.GetStatus()).ToList();
            var save = SaveManager.LoadByXML(string.Concat(name, ".txt"), defaultSave) as List<UnlockStatus>;
            for (var i = 0; i < save.Count; i++)
            {
                unlockableSos[i].SetUnlocked(save[i]);
            }
        }

        public List<UnlockableSo> GetUnlockedSos() => unlockableSos.FindAll(unlockableSo => unlockableSo.Unlocked());
    }
}