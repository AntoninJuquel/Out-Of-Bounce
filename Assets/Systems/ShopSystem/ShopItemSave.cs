using System;
using UnityEngine;

namespace ShopSystem
{
    [Serializable]
    public class ShopItemSave
    {
        [field: SerializeField] public string Title { get; private set; }
        [field: SerializeField] public bool Selected { get; private set; }
        [field: SerializeField] public int Level { get; private set; }
        [field: SerializeField] public LockState[] LockStates { get; private set; }

        public ShopItemSave(string title, bool selected, int level, LockState[] lockStates)
        {
            Title = title;
            Selected = selected;
            Level = level;
            LockStates = lockStates;
        }
    }
}