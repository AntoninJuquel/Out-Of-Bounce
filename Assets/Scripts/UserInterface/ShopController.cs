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
        [SerializeField] private GameObject shopItemPrefab;
        [SerializeField] private RectTransform content;
        [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;
        [SerializeField] private TextMeshProUGUI money;
        [SerializeField] private PlayerSo playerSo;
        [SerializeField] private CanvasController canvasController;
        public void SetupShop(string shopName)
        {
            var unlockableSos = new List<UnlockableSo>();
            switch (shopName)
            {
                case "dots":
                    unlockableSos = playerSo.GetDots().Cast<UnlockableSo>().ToList();
                    break;
            }
            
            verticalLayoutGroup.enabled = true;

            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }

            foreach (var unlockableSo in unlockableSos)
            {
                var shopItem = Instantiate(shopItemPrefab, content);
                shopItem.GetComponent<ShopItemController>().Setup(unlockableSo, this);
            }

            canvasController.SetActiveMenu(gameObject);
            verticalLayoutGroup.enabled = false;

            money.text = string.Concat(playerSo.GetMoney(), " $");
        }

        public void UpdateShop()
        {
            money.text = string.Concat(playerSo.GetMoney(), " $");
            playerSo.SavePlayer();
        }
    }
}