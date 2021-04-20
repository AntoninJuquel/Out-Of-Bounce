using Systems.Save;
using Systems.Unlock;
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
        private UnlockableDataBaseSo _unlockableDataBaseSo;

        public void SetupShop(UnlockableDataBaseSo unlockableDataBaseSo)
        {
            _unlockableDataBaseSo = unlockableDataBaseSo;
            verticalLayoutGroup.enabled = true;

            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }

            foreach (var unlockableSo in unlockableDataBaseSo.unlockableSos)
            {
                var shopItem = Instantiate(shopItemPrefab, content);
                shopItem.GetComponent<ShopItemController>().Setup(unlockableSo, this);
            }

            verticalLayoutGroup.enabled = false;

            money.text = string.Concat(playerSo.GetMoney(), " $");
        }

        public void UpdateShop()
        {
            money.text = string.Concat(playerSo.GetMoney(), " $");
            _unlockableDataBaseSo.SaveUnlockables();
            playerSo.SavePlayer();
        }
    }
}