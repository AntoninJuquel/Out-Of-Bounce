using Systems.Event.Scripts.Channels;
using UnityEngine;
using UnityEngine.Events;

namespace Systems.Event.Scripts.Listeners
{
    public class FloatEventListener : MonoBehaviour
    {
        [System.Serializable]
        private class FloatEvent
        {
            [SerializeField] private FloatEventChannelSo channel = default;
            [SerializeField] UnityEvent<float> OnEventRaised;
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
            private void Respond(float value) => OnEventRaised?.Invoke(value);
        }
        [SerializeField]
        private FloatEvent[] floatEvents = new FloatEvent[] { };
        private void OnEnable()
        {
            foreach (var floatEvent in floatEvents)
                floatEvent.Enable();
        }

        private void OnDisable()
        {
            foreach (var floatEvent in floatEvents)
                floatEvent.Disable();
        }
    }
}