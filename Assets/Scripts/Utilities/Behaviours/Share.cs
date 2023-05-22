using UnityEngine;

namespace Utilities.Behaviours
{
    public class Share : MonoBehaviour
    {
        public void ShareGame()
        {
#if UNITY_ANDROID || UNITY_IOS
            new NativeShare()
                .SetTitle("Partager Out of Bounce")
                .SetText("Jouez à Out of Bounce et marquez le plus de points possible : ")
                .SetUrl("https://play.google.com/store/apps/details?id=com.kibblecorp.outofbounce")
                .Share();
#else
            GUIUtility.systemCopyBuffer = "https://somindras.itch.io/out-of-bounce";
#endif
        }
    }
}