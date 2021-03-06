using System.Collections.Generic;
using System.Linq;
using Systems.Ads;
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
            var skins = new List<SkinSo>();
            foreach (var s in playerSo.GetSkinSets().Select(skinSet => skinSet.GetSkins().FindAll(skin => skin.GetSkinType() == skinType)))
            {
                skins.AddRange(s);
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
                    var skinAd = skinTrans.GetChild(6).GetComponent<RewardedAdsButton>();
                    var skinAdButton = skinTrans.GetChild(6).GetComponent<Button>();

                    skinButton.onClick.RemoveAllListeners();
                    skinToggle.onValueChanged.RemoveAllListeners();
                    skinAdButton.onClick.RemoveAllListeners();
                    skinAd.onFinished.RemoveAllListeners();

                    skinTrans.gameObject.SetActive(inSkinRange);

                    if (!inSkinRange) continue;
                    var skin = skins[i];
                    skinName.text = skin.name;
                    skinFrame.color = skin.Unlocked() ? Color.green : Color.red;
                    skinImage.rectTransform.sizeDelta = new Vector2(480, 480) * (skinType == SkinType.Platforms || skinType == SkinType.Trails || skinType == SkinType.Particles ? .5f : 1);
                    skinImage.sprite = skin.GetSprites()[0];
                    skinPrice.text = string.Concat(skin.GetPrice(), "$");

                    skinButton.gameObject.SetActive(!skin.Unlocked());
                    skinButton.onClick.AddListener(delegate { HandleBuySkin(skinFrame, skin, skinToggle, skinButton, skinAdButton); });

                    skinToggle.gameObject.SetActive(skin.Unlocked());
                    skinToggle.isOn = skin.Selected();
                    skinToggle.onValueChanged.AddListener(delegate(bool isOn) { skin.SetSelected(isOn); });
                    
                    skinAdButton.onClick.AddListener(delegate
                    {
                        skinAd.ShowRewardedVideo();
                        HandleAd(skinFrame, skin, skinToggle, skinButton, skinAd);
                    });
                    skinAdButton.interactable = !skin.Unlocked();
                }
            }
        }

        private void OnEnable()
        {
            moneyText.text = string.Concat(playerSo.GetMoney(), "$");
            HandleCategory(SkinType.Balls);
        }

        private void UpdateVisuals(Graphic skinFrame, UnlockableSo skin, Toggle skinToggle, Button skinButton, Button skinAd)
        {
            skinFrame.color = skin.Unlocked() ? Color.green : Color.red;
            moneyText.text = string.Concat(playerSo.GetMoney(), "$");
            skinButton.gameObject.SetActive(!skin.Unlocked());
            skinAd.interactable = !skin.Unlocked();
            skinToggle.gameObject.SetActive(skin.Unlocked());
            skinToggle.isOn = true;
        }
        private void HandleBuySkin(Graphic skinFrame, UnlockableSo skin, Toggle skinToggle, Button skinButton, Button skinAd)
        {
            skin.Unlock(playerSo.GetVault());
            playerSo.SavePlayer();
            UpdateVisuals(skinFrame, skin, skinToggle, skinButton, skinAd);
        }


        private void HandleAd(Graphic skinFrame, UnlockableSo skin, Toggle skinToggle, Button skinButton, RewardedAdsButton skinAd)
        {
            skinAd.onFinished.AddListener(delegate
            {
                skin.SetUnlocked(UnlockStatus.Unlocked);
                playerSo.SavePlayer();
                UpdateVisuals(skinFrame, skin, skinToggle, skinButton, skinAd.GetComponent<Button>());
                skinAd.onFinished.RemoveAllListeners();
            });
        }
    }
}