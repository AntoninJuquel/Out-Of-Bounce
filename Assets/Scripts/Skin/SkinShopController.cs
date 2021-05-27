using System.Linq;
using Systems.Unlock;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Skin
{
    public class SkinShopController : MonoBehaviour
    {
        [SerializeField] private PlayerSo playerSo;
        [SerializeField] private RectTransform categoriesMenu, skinsMenu;
        [SerializeField] private GameObject skinItem, categoryButton;
        [SerializeField] private TextMeshProUGUI moneyText;

        private void Awake()
        {
            foreach (var skinType in SkinUtilities.SkinTypesArray())
            {
                var categoryBtn = Instantiate(categoryButton, categoriesMenu);
                categoryBtn.GetComponent<Button>().onClick.AddListener(delegate { HandleCategory(skinType); });
                categoryBtn.GetComponentInChildren<TextMeshProUGUI>().text = skinType.ToString();
                categoryBtn.SetActive(true);
            }
        }

        private void HandleCategory(SkinType skinType)
        {
            foreach (var skins in playerSo.GetSkinSets().Select(skinSet => skinSet.GetSkins().FindAll(skin => skin.GetSkinType() == skinType)))
            {
                for (var i = 0; i < Mathf.Max(skins.Count, skinsMenu.childCount); i++)
                {
                    var skinTrans = i < skinsMenu.childCount ? skinsMenu.GetChild(i) : Instantiate(skinItem, skinsMenu).transform;
                    var inSkinRange = i < skins.Count;

                    var skinName = skinTrans.GetChild(0).GetComponent<TextMeshProUGUI>();
                    var skinFrame = skinTrans.GetChild(1).GetComponent<Image>();
                    var skinImage = skinTrans.GetChild(2).GetComponent<Image>();
                    var skinPrice = skinTrans.GetChild(3).GetComponent<TextMeshProUGUI>();
                    var skinButton = skinTrans.GetChild(4).GetComponent<Button>();
                    var skinToggle = skinTrans.GetChild(5).GetComponent<Toggle>();

                    skinButton.onClick.RemoveAllListeners();
                    skinToggle.onValueChanged.RemoveAllListeners();
                    
                    skinTrans.gameObject.SetActive(inSkinRange);

                    if (!inSkinRange) continue;
                    var skin = skins[i];
                    skinName.text = skin.name;
                    skinFrame.color = skin.Unlocked() ? Color.green : Color.red;
                    skinImage.rectTransform.sizeDelta = new Vector2(480, 480) * (skinType == SkinType.Platform || skinType == SkinType.Trail || skinType == SkinType.Particles ? .5f : 1);
                    skinImage.sprite = skin.GetSprites()[0];
                    skinPrice.text = string.Concat(skin.GetPrice(), "$");

                    skinButton.gameObject.SetActive(!skin.Unlocked());
                    skinButton.onClick.AddListener(delegate { HandleBuySkin(skinFrame, skin, skinToggle, skinButton); });

                    skinToggle.gameObject.SetActive(skin.Unlocked());
                    skinToggle.isOn = skin.Selected();
                    skinToggle.onValueChanged.AddListener(delegate(bool isOn) { skin.SetSelected(isOn); });
                }
            }
        }

        private void OnEnable()
        {
            moneyText.text = string.Concat(playerSo.GetMoney(), "$");
        }

        private void HandleBuySkin(Graphic skinFrame, UnlockableSo skin, Toggle skinToggle, Button skinButton)
        {
            skin.Unlock(playerSo.GetVault());
            skinFrame.color = skin.Unlocked() ? Color.green : Color.red;
            playerSo.SavePlayer();
            moneyText.text = string.Concat(playerSo.GetMoney(), "$");
            
            skinButton.gameObject.SetActive(!skin.Unlocked());
            skinToggle.gameObject.SetActive(skin.Unlocked());
            skinToggle.isOn = true;
        }
    }
}