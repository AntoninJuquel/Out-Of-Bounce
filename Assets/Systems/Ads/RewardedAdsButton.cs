using Managers;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

namespace Systems.Ads
{
    [RequireComponent(typeof(Button))]
    public class RewardedAdsButton : MonoBehaviour, IUnityAdsListener
    {
#if UNITY_IOS
    private const string GameId = "4105544";
#elif UNITY_ANDROID
        private const string GameId = "4105545";
#endif

        private Button _myButton;
        private const string MySurfacingId = "rewardedVideo";

        private void Start()
        {
            _myButton = GetComponent<Button>();

            // Set interactivity to be dependent on the Ad Unit or legacy Placement’s status:
            _myButton.interactable = Advertisement.IsReady(MySurfacingId);

            // Map the ShowRewardedVideo function to the button’s click listener:
            if (_myButton) _myButton.onClick.AddListener(ShowRewardedVideo);

            // Initialize the Ads listener and service:
            Advertisement.AddListener(this);
            Advertisement.Initialize(GameId, false);
        }

        // Implement a function for showing a rewarded video ad:
        private void ShowRewardedVideo()
        {
            Advertisement.Show(MySurfacingId);
        }

        // Implement IUnityAdsListener interface methods:
        public void OnUnityAdsReady(string surfacingId)
        {
            // If the ready Ad Unit or legacy Placement is rewarded, activate the button: 
            if (surfacingId == MySurfacingId)
            {
                _myButton.interactable = true;
            }
        }

        public void OnUnityAdsDidFinish(string surfacingId, ShowResult showResult)
        {
            // Define conditional logic for each ad completion status:
            if (showResult == ShowResult.Finished)
            {
                GameManager.Instance.Respawn();
            }
            else if (showResult == ShowResult.Skipped)
            {
                // Do not reward the user for skipping the ad.
            }
            else if (showResult == ShowResult.Failed)
            {
                Debug.LogWarning("The ad did not finish due to an error.");
            }
        }

        public void OnUnityAdsDidError(string message)
        {
            // Log the error.
        }

        public void OnUnityAdsDidStart(string surfacingId)
        {
            // Optional actions to take when the end-users triggers an ad.
        }
    }
}