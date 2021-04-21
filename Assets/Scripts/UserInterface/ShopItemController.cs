using Systems.Unlock;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class ShopItemController : MonoBehaviour
    {
        [SerializeField] private PlayerSo playerSo;
        [SerializeField] private Image image, frame;
        [SerializeField] private TextMeshProUGUI title, price, description;
        [SerializeField] private Button buyButton;
        private UnlockableSo _unlockableSo;
        private ShopController _shopController;

        public void Setup(UnlockableSo unlockableSo, ShopController shopController)
        {
            _unlockableSo = unlockableSo;
            _shopController = shopController;
            image.sprite = unlockableSo.GetSprites()[0];
            image.color = unlockableSo.GetColor();
            frame.color = unlockableSo.Unlocked() ? Color.green : Color.red;
            price.text = string.Concat(unlockableSo.GetPrice(), " $");
            title.text = unlockableSo.name;
            description.text = unlockableSo.GetDescription();
            buyButton.interactable = !unlockableSo.Unlocked();
        }

        public void Buy()
        {
            _unlockableSo.Unlock(playerSo.GetVault());
            Setup(_unlockableSo, _shopController);
            _shopController.UpdateShop();
        }
    }
}