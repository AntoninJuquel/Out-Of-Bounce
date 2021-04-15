using Systems.EventSystem.Scripts.Channels;
using UnityEngine;
using UnityEngine.Events;

namespace Systems.EventSystem.Scripts.Listeners
{
    public class VoidEventListener : MonoBehaviour
    {
        [System.Serializable]
        private class VoidEvent
        {
            [SerializeField] private VoidEventChannelSO channel = default;
            [SerializeField] UnityEvent OnEventRaised;
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
            private void Respond() => OnEventRaised?.Invoke();
        }
        [SerializeField] 
        private VoidEvent[] voidEvents = new VoidEvent[] { };
        private void OnEnable()
        {
            foreach (var voidEvent in voidEvents)
                voidEvent.Enable();
        }

        private void OnDisable()
        {
            foreach (var voidEvent in voidEvents)
                voidEvent.Disable();
        }
    }
}
