using UnityEngine;
using UnityEngine.Events;

namespace ShopSystem
{
    [CreateAssetMenu(fileName = "New wallet", menuName = "ShopSystem/Wallet", order = 0)]
    public class Wallet : ScriptableObject
    {
        [field: SerializeField] public int Coins { get; private set; }
        public UnityEvent<int> onCoinsChanged;

        public void SetCoins(int amount)
        {
            Coins = amount;
        }

        public void AddCoins(int amount)
        {
            Coins += amount;
            onCoinsChanged?.Invoke(Coins);
        }

        public void RemoveCoins(int amount)
        {
            Coins -= amount;
            onCoinsChanged?.Invoke(Coins);
        }

        public bool CanAfford(int amount)
        {
            return Coins >= amount;
        }
    }
}