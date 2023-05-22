using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using ShopSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class ShopItemCard : SerializedMonoBehaviour
    {
        [SerializeField] private Wallet wallet;
        [SerializeField] private GameObject priceContainer;
        [SerializeField] private TextMeshProUGUI title, description, price, buttonText, levelText;
        [SerializeField] private Image icon, buttonImage, buttonIcon, levelFill;
        [SerializeField] private UIButton button;
        [SerializeField] private UIToggle toggle;
        [SerializeField] private MMF_Player buyFeedback, cantBuyFeedback;

        [SerializeField] private Dictionary<LockState, Sprite> lockStateIcons = new();
        [SerializeField] private Dictionary<LockState, Sprite> lockStateButtons = new();
        [SerializeField] private Dictionary<bool, Sprite> affordableIcons = new();
        [SerializeField] private Dictionary<bool, Sprite> affordableButtons = new();
        [SerializeField] private Sprite levelSprite, maxLevelButton, maxLevelIcon;
        [SerializeField] private string levelTextFormat = "{0}/{1}", maxLevelText = "Max Level", buyText = "Buy";

        private ShopItem _shopItem;
        private bool _isInitialized;

        private Sprite SelectButtonSprite()
        {
            if (_shopItem.IsMaxLevel)
            {
                return maxLevelButton;
            }

            return _shopItem.Unlocked
                ? affordableButtons[wallet.CanAfford(_shopItem.NextLevel.Price)]
                : lockStateButtons[_shopItem.NextLevel.LockState];
        }

        private Sprite SelectIconSprite()
        {
            if (_shopItem.IsMaxLevel)
            {
                return maxLevelIcon;
            }

            return _shopItem.Unlocked
                ? affordableIcons[wallet.CanAfford(_shopItem.NextLevel.Price)]
                : lockStateIcons[_shopItem.NextLevel.LockState];
        }

        public void Init(ShopItem shopItem)
        {
            _isInitialized = false;

            _shopItem = shopItem;
            title.text = shopItem.Title.ToUpper();
            description.text =
                $"Current:\n{shopItem.CurrentLevel.Description}\n\nNext:\n{shopItem.NextLevel.Description}";
            priceContainer.SetActive(!shopItem.IsMaxLevel);
            price.text = shopItem.NextLevel.Price.ToString();
            icon.sprite = shopItem.Icon;
            buttonImage.sprite = SelectButtonSprite();
            buttonIcon.sprite = SelectIconSprite();
            levelText.text = shopItem.IsMaxLevel
                ? maxLevelText
                : string.Format(levelTextFormat, shopItem.Level, shopItem.LevelsCount);
            levelFill.sprite = shopItem.IsMaxLevel ? maxLevelButton : levelSprite;
            var progress = (shopItem.Level) / (float)shopItem.LevelsCount;
            levelFill.rectTransform.offsetMax = new Vector2(250f * (progress - 1f), 0);
            button.interactable = shopItem.NextLevel.LockState == LockState.Unlocked && !shopItem.IsMaxLevel;
            buttonText.text = shopItem.IsMaxLevel ? maxLevelText : buyText;

            toggle.isOn = shopItem.Selected;
            toggle.interactable = shopItem.Selectable;
            toggle.gameObject.SetActive(toggle.interactable);

            _isInitialized = true;
        }

        public void OnClickPurchase()
        {
            if (!_isInitialized)
            {
                return;
            }

            var succeeded = _shopItem.PurchaseNext(wallet);

            if (succeeded)
            {
                buyFeedback.PlayFeedbacks();
            }
            else
            {
                cantBuyFeedback.PlayFeedbacks();
            }

            Init(_shopItem);
        }

        public void OnToggleEquip(bool isOn)
        {
            if (!_isInitialized)
            {
                return;
            }

            if (isOn)
            {
                _shopItem.Select();
            }
            else
            {
                _shopItem.Deselect();
            }
        }
    }
}