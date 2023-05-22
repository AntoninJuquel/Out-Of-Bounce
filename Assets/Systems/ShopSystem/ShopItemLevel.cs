using UnityEngine;

namespace ShopSystem
{
    [System.Serializable]
    public class ShopItemLevel
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Image { get; private set; }
        [field: SerializeField] public int Price { get; private set; }
        [field: SerializeField] public LockState LockState { get; private set; }
        [SerializeField] [TextArea(10, 30)] private string description;
        public string Description => description;

        public ShopItemLevel(string name)
        {
            Name = name;
        }

        public ShopItemLevel(string name, Sprite image)
        {
            Name = name;
            Image = image;
        }

        public ShopItemLevel(string name, Sprite image, int price, LockState lockState)
        {
            Name = name;
            Image = image;
            Price = price;
            LockState = lockState;
        }

        public void Purchase()
        {
            LockState = LockState.Purchased;
        }

        public void Reset()
        {
            LockState = LockState.Unlocked;
        }

        public void Load(LockState lockState)
        {
            LockState = lockState;
        }
    }
}