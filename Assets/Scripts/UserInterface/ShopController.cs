using System.Collections.Generic;
using System.Linq;
using Systems.Unlock;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class ShopController : MonoBehaviour
    {
        [SerializeField] private PlayerSo playerSo;
        [SerializeField] private RectTransform shopMenu;
        [SerializeField] private GameObject shopItem;
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private Scrollbar shopScroll;

        public void SetupShop(string shopName)
        {
            shopScroll.value = 0;
            moneyText.text = string.Concat(playerSo.GetMoney(), "$");
            var unlockableSos = new List<UnlockableSo>();
            switch (shopName)
            {
                case "dots":
                    unlockableSos = playerSo.GetDots().Cast<UnlockableSo>().ToList();
                    break;
                case "upgrades":
                    unlockableSos = playerSo.GetUpgrades().Cast<UnlockableSo>().ToList();
                    break;
            }

            for (var i = 0; i < Mathf.Max(unlockableSos.Count, shopMenu.childCount); i++)
            {
                var unlockableTrans = i < shopMenu.childCount ? shopMenu.GetChild(i) : Instantiate(shopItem, shopMenu).transform;
                var inUnlockableRange = i < unlockableSos.Count;

                var unlockableName = unlockableTrans.GetChild(0).GetComponent<TextMeshProUGUI>();
                var unlockableFrame = unlockableTrans.GetChild(1).GetComponent<Image>();
                var unlockableImage = unlockableTrans.GetChild(2).GetComponent<Image>();
                var unlockablePrice = unlockableTrans.GetChild(3).GetComponent<TextMeshProUGUI>();
                var unlockableButton = unlockableTrans.GetChild(4).GetComponent<Button>();
                var unlockableDescription = unlockableTrans.GetChild(5).GetComponent<TextMeshProUGUI>();

                unlockableButton.onClick.RemoveAllListeners();

                unlockableTrans.gameObject.SetActive(inUnlockableRange);

                if (!inUnlockableRange) continue;

                var unlockable = unlockableSos[i];
                unlockableName.text = unlockable.name;
                unlockableFrame.color = unlockable.Unlocked() ? Color.green : Color.red;
                unlockableImage.sprite = unlockable.GetSprites()[0];
                unlockableImage.color = unlockable.GetColor();
                unlockablePrice.text = string.Concat(unlockable.GetPrice(), "$");
                unlockableDescription.text = unlockable.GetDescription();
                unlockableButton.interactable = !unlockable.MaxLevel() || !unlockable.Unlocked();

                unlockableButton.onClick.AddListener(delegate { HandleBuyUnlockable(unlockableFrame, unlockable, unlockableButton, unlockableDescription); });
            }
        }

        void HandleBuyUnlockable(Image unlockableFrame, UnlockableSo unlockable, Button unlockableButton, TextMeshProUGUI unlockableDescription)
        {
            if(unlockable.Unlocked())
                unlockable.Upgrade(playerSo.GetVault());
            else
                unlockable.Unlock(playerSo.GetVault());
            playerSo.SavePlayer();
            moneyText.text = string.Concat(playerSo.GetMoney(), "$");
            unlockableFrame.color = unlockable.Unlocked() ? Color.green : Color.red;
            unlockableButton.interactable = !unlockable.MaxLevel() || !unlockable.Unlocked();
            unlockableDescription.text = unlockable.GetDescription();
        }
    }
}