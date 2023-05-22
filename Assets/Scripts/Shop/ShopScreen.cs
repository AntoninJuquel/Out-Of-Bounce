using ShopSystem;
using TMPro;
using UnityEngine;

namespace Shop
{
    public class ShopScreen : MonoBehaviour
    {
        [SerializeField] private Wallet wallet;
        [SerializeField] private ShopItemCard shopItemCardPrefab;
        [SerializeField] private RectTransform shopContainer;
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private ShopItem[] shopItems;

        private void OnEnable()
        {
            wallet.onCoinsChanged.AddListener(UpdateMoneyText);
            UpdateMoneyText(wallet.Coins);
        }

        private void OnDisable()
        {
            wallet.onCoinsChanged.RemoveListener(UpdateMoneyText);
        }

        private void Start()
        {
            foreach (var shopItem in shopItems)
            {
                var shopItemCard = Instantiate(shopItemCardPrefab, shopContainer);
                shopItemCard.Init(shopItem);
            }

            moneyText.text = wallet.Coins.ToString();
            shopContainer.GetChild(0).gameObject.SetActive(false);
        }

        private void UpdateMoneyText(int coins)
        {
            moneyText.text = coins.ToString();
        }
    }
}