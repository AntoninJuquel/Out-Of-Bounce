using System;
using System.Collections.Generic;
using Save;
using ShopSystem;
using UnityEngine;

namespace Skins
{
    [CreateAssetMenu(fileName = "New skin", menuName = "Skins/Skin", order = 0)]
    public class SkinItem : ShopItem, ISave
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }

        private void OnEnable()
        {
            onItemChange.AddListener(OnItemChange);
        }

        private void OnDisable()
        {
            onItemChange.RemoveListener(OnItemChange);
        }

        private void OnItemChange(ShopItemSave save, string saveName)
        {
            Save();
        }

        public event Action<string, object> OnSave;
        public object DefaultSave => SaveItem();
        public string Name => name;

        public void Load(object loadedObject)
        {
            if (loadedObject is ShopItemSave save)
            {
                LoadItem(save);
            }
        }

        public void Save()
        {
            OnSave?.Invoke(Name, SaveItem());
        }
    }
}