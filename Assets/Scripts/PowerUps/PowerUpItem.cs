using System;
using Save;
using ShopSystem;
using UnityEngine;

namespace PowerUp
{
    public abstract class PowerUpItem : ShopItem, ISave
    {
        public virtual void OnEnablePowerUp(GameObject gameObject)
        {
        }

        public virtual void UpdatePowerUp(GameObject gameObject)
        {
        }

        public virtual void FixedUpdatePowerUp(GameObject gameObject)
        {
        }

        public virtual void OnCollisionEnter2DPowerUp(GameObject gameObject, Collision2D other)
        {
        }

        public virtual void OnTriggerEnter2DPowerUp(GameObject gameObject, Collider2D other)
        {
        }

        public virtual void OnDisablePowerUp(GameObject gameObject)
        {
        }

        public virtual void OnBouncePowerUp(GameObject gameObject, GameObject other)
        {
        }

        public virtual void TriggerPowerUp(GameObject gameObject)
        {
        }

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