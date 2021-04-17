using Systems.Event.Scripts.Channels;
using UnityEngine;
using UnityEngine.Events;

namespace Systems.Event.Scripts.Listeners
{
    public class BoolEventListener : MonoBehaviour
    {
        [System.Serializable]
        private class BoolEvent
        {
            [SerializeField] private BoolEventChannelSo channel = default;
            [SerializeField] UnityEvent<bool> OnEventRaised;
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
            private void Respond(bool value) => OnEventRaised?.Invoke(value);
        }
        [SerializeField]
        private BoolEvent[] boolEvents = new BoolEvent[] { };
        private void OnEnable()
        {
            foreach (var boolEvent in boolEvents)
                boolEvent.Enable();
        }

        private void OnDisable()
        {
            foreach (var boolEvent in boolEvents)
                boolEvent.Disable();
        }
    }
}