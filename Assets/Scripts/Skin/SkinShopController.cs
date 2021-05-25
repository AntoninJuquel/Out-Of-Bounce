using System;
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
            }

            categoryButton.SetActive(false);
            skinItem.SetActive(false);
        }

        private void HandleCategory(SkinType skinType)
        {
            foreach (var skins in playerSo.GetSkinSets().Select(skinSet => skinSet.GetSkins().FindAll(skin => skin.GetSkinType() == skinType)))
            {
                for (var i = 0; i < Mathf.Max(skins.Count, skinsMenu.childCount); i++)
                {
                    var skinTrans = i < skinsMenu.childCount ? skinsMenu.GetChild(i) : Instantiate(skinItem, skinsMenu).transform;
                    var inSkinRange = i < skins.Count;

                    var skiFrame = skinTrans.GetChild(1).GetComponent<Image>();
                    var skinButton = skinTrans.GetChild(4).GetComponent<Button>();
                    var skinToggle = skinTrans.GetChild(5).GetComponent<Toggle>();

                    skinButton.onClick.RemoveAllListeners();
                    skinToggle.onValueChanged.RemoveAllListeners();
                    
                    skinTrans.gameObject.SetActive(inSkinRange);

                    if (!inSkinRange) continue;
                    var skin = skins[i];
                    skinTrans.GetChild(0).GetComponent<TextMeshProUGUI>().text = skin.name;
                    skiFrame.color = skin.Unlocked() ? Color.green : Color.red;
                    skinTrans.GetChild(2).GetComponent<Image>().sprite = skin.GetSprites()[0];
                    skinTrans.GetChild(3).GetComponent<TextMeshProUGUI>().text = string.Concat(skin.GetPrice(), "$");

                    skinButton.gameObject.SetActive(!skin.Unlocked());
                    skinButton.onClick.AddListener(delegate { HandleBuySkin(skiFrame, skin, skinToggle, skinButton); });

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