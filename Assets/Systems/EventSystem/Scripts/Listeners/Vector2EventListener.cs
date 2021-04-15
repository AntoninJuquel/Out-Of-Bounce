using Systems.EventSystem.Scripts.Channels;
using UnityEngine;
using UnityEngine.Events;

namespace Systems.EventSystem.Scripts.Listeners
{
    public class Vector2EventListener : MonoBehaviour
    {
        [System.Serializable]
        private class Vector2Event
        {
            [SerializeField] private Vector2EventChannelSO channel = default;
            [SerializeField] UnityEvent<Vector2> OnEventRaised;
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
            private void Respond(Vector2 value) => OnEventRaised?.Invoke(value);
        }
        [SerializeField]
        private Vector2Event[] Vector2Events = new Vector2Event[] { };
        private void OnEnable()
        {
            foreach (var Vector2Event in Vector2Events)
                Vector2Event.Enable();
        }

        private void OnDisable()
        {
            foreach (var Vector2Event in Vector2Events)
                Vector2Event.Disable();
        }
    }
}