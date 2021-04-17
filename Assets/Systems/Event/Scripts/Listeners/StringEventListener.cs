using Systems.Event.Scripts.Channels;
using UnityEngine;
using UnityEngine.Events;

namespace Systems.Event.Scripts.Listeners
{
    public class StringEventListener : MonoBehaviour
    {
        [System.Serializable]
        private class StringEvent
        {
            [SerializeField] private StringEventChannelSo channel = default;
            [SerializeField] UnityEvent<string> OnEventRaised;
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
            private void Respond(string value) => OnEventRaised?.Invoke(value);
        }
        [SerializeField]
        private StringEvent[] stringEvents = new StringEvent[] { };
        private void OnEnable()
        {
            foreach (var stringEvent in stringEvents)
                stringEvent.Enable();
        }

        private void OnDisable()
        {
            foreach (var stringEvent in stringEvents)
                stringEvent.Disable();
        }
    }
}