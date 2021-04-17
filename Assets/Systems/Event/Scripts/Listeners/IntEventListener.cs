using Systems.Event.Scripts.Channels;
using UnityEngine;
using UnityEngine.Events;

namespace Systems.Event.Scripts.Listeners
{
    public class IntEventListener : MonoBehaviour
    {
        [System.Serializable]
        private class IntEvent
        {
            [SerializeField] private IntEventChannelSo channel = default;
            [SerializeField] UnityEvent<int> OnEventRaised;
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
            private void Respond(int value) => OnEventRaised?.Invoke(value);
        }
        [SerializeField]
        private IntEvent[] intEvents = new IntEvent[] { };
        private void OnEnable()
        {
            foreach (var intEvent in intEvents)
                intEvent.Enable();
        }

        private void OnDisable()
        {
            foreach (var intEvent in intEvents)
                intEvent.Disable();
        }
    }
}