using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Systems.Ads
{
#if UNITY_ANDROID || UNITY_IOS
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
        [SerializeField] private UnityEvent onFinished, onSkipped, onFailed;

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
            switch (showResult)
            {
                // Define conditional logic for each ad completion status:
                case ShowResult.Finished:
                    onFinished?.Invoke();
                    break;
                case ShowResult.Skipped:
                    onSkipped?.Invoke();
                    break;
                case ShowResult.Failed:
                    Debug.LogWarning("The ad did not finish due to an error.");
                    onFailed?.Invoke();
                    break;
                default:
                    break;
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
#else
    public class RewardedAdsButton : MonoBehaviour
    {
        private void OnEnable()
        {
            gameObject.SetActive(false);
        }
    }
#endif
}