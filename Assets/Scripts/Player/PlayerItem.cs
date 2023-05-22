using System;
using Save;
using ShopSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "Player data", menuName = "Player", order = 0)]
    public class PlayerItem : ScriptableObject, ISave
    {
        [SerializeField] private Wallet wallet;
        public event Action<string, object> OnSave;

        public object DefaultSave => new PlayerSave(wallet.Coins);

        public string Name => name;

        public void Load(object loadedObject)
        {
            if (loadedObject is PlayerSave playerSave)
            {
                wallet.SetCoins(playerSave.Coins);
            }
        }

        [Button]
        public void Save()
        {
            OnSave?.Invoke(Name, new PlayerSave(wallet.Coins));
        }
    }
}