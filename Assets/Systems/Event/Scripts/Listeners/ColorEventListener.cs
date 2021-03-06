using Systems.Event.Scripts.Channels;
using UnityEngine;
using UnityEngine.Events;

namespace Systems.Event.Scripts.Listeners
{
    public class ColorEventListener : MonoBehaviour
    {
        [System.Serializable]
        private class ColorEvent
        {
            [SerializeField] private ColorEventChannelSo channel = default;
            [SerializeField] UnityEvent<Color> OnEventRaised;
            public void Enable()
            {
                if (channel != null)
                    channel.OnEventRaised += Respond;
            }
            public void Disable()
            {
                if (channel != null)
                    channel.OnEventRaised -= Respond;
            }
            private void Respond(Color value) => OnEventRaised?.Invoke(value);
        }
        [SerializeField]
        private ColorEvent[] ColorEvents = new ColorEvent[] { };
        private void OnEnable()
        {
            foreach (var ColorEvent in ColorEvents)
                ColorEvent.Enable();
        }

        private void OnDisable()
        {
            foreach (var ColorEvent in ColorEvents)
                ColorEvent.Disable();
        }
    }
}