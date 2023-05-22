using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace ShopSystem
{
    [System.Serializable]
    public class ShopItem : ScriptableObject
    {
        [InlineButton("AutofillTitle", "Autofill")] [SerializeField]
        private string title;

        [PreviewField(64, ObjectFieldAlignment.Left)] [SerializeField]
        private Sprite image;

        [field: SerializeField] public int Level { get; private set; }

        [InlineButton("QuickAdd", "+")] [SerializeField]
        private ShopItemLevel[] levels;

        [SerializeField] private bool selectable;
        [field: SerializeField] public bool Selected { get; private set; }

        [SerializeField] protected UnityEvent<ShopItemSave, string> onItemChange;

        public ShopItemLevel CurrentLevel
        {
            get
            {
                if (levels == null || levels.Length == 0)
                {
                    levels = new[] { new ShopItemLevel(title) };
                }

                return Level == 0 ? levels[0] : levels[Level - 1];
            }
        }

        public ShopItemLevel NextLevel
        {
            get
            {
                if (levels == null || levels.Length == 0)
                {
                    levels = new[] { new ShopItemLevel(title) };
                }

                return IsMaxLevel ? levels[Level - 1] : levels[Level];
            }
        }

        public int LevelIndex => Level - 1;
        public int LevelsCount => levels.Length;
        public bool IsMaxLevel => Level == LevelsCount;
        public string Title => NextLevel.Name == string.Empty ? title : NextLevel.Name;
        public Sprite Icon => NextLevel.Image == null ? image : NextLevel.Image;
        public bool Unlocked => NextLevel.LockState == LockState.Unlocked;
        public bool Purchased => Level > 0 || NextLevel.LockState == LockState.Purchased;
        public bool Selectable => selectable && Level > 0 && levels[Level - 1].LockState == LockState.Purchased;

        #region Editor functions

        private void AutofillTitle()
        {
            title = name;
        }

        private void QuickAdd()
        {
            var newLevels = new ShopItemLevel[levels.Length + 1];
            levels.CopyTo(newLevels, 0);
            newLevels[levels.Length] = new ShopItemLevel(title, image, 0, LockState.Unlocked);
            levels = newLevels;
        }

        #endregion

        [Button]
        public void PurchaseNext()
        {
            if (IsMaxLevel)
            {
                return;
            }

            levels[Level].Purchase();
            Selected = true;
            Level++;
            onItemChange?.Invoke(SaveItem(), name);
        }

        [Button]
        public bool PurchaseNext(Wallet wallet)
        {
            if (IsMaxLevel)
            {
                return false;
            }

            if (levels[Level].LockState != LockState.Unlocked)
            {
                return false;
            }

            if (!wallet.CanAfford(levels[Level].Price))
            {
                return false;
            }

            wallet.RemoveCoins(NextLevel.Price);
            PurchaseNext();
            return true;
        }

        public void PurchaseAll()
        {
            if (IsMaxLevel)
            {
                return;
            }

            while (Level < LevelsCount)
            {
                PurchaseNext();
            }

            Level = LevelsCount;
            onItemChange?.Invoke(SaveItem(), name);
        }

        public void PurchaseAll(Wallet wallet)
        {
            if (IsMaxLevel)
            {
                return;
            }

            if (levels.Any(item => item.LockState != LockState.Unlocked))
            {
                return;
            }

            var totalPrice = levels.Skip(Level).Sum(item => item.Price);

            if (!wallet.CanAfford(totalPrice))
            {
                return;
            }

            wallet.RemoveCoins(totalPrice);
            PurchaseAll();
        }

        [Button]
        public void Select()
        {
            if (!selectable)
            {
                return;
            }

            Selected = true;
            onItemChange?.Invoke(SaveItem(), name);
        }

        [Button]
        public void Deselect()
        {
            if (!selectable)
            {
                return;
            }

            Selected = false;
            onItemChange?.Invoke(SaveItem(), name);
        }

        [Button]
        public void ToggleSelect()
        {
            if (!selectable)
            {
                return;
            }

            Selected = !Selected;
            onItemChange?.Invoke(SaveItem(), name);
        }

        [Button]
        public void Reset()
        {
            Level = 0;
            Selected = false;
            foreach (var level in levels)
            {
                level.Reset();
            }

            onItemChange?.Invoke(SaveItem(), name);
        }

        [Button]
        public ShopItemSave SaveItem()
        {
            return new ShopItemSave(name, Selected, Level, levels.Select(level => level.LockState).ToArray());
        }

        [Button]
        public void LoadItem(ShopItemSave save)
        {
            Selected = save.Selected;
            Level = save.Level;
            for (var i = 0; i < levels.Length; i++)
            {
                levels[i].Load(i < save.LockStates.Length ? save.LockStates[i] : LockState.Unlocked);
            }
        }
    }
}