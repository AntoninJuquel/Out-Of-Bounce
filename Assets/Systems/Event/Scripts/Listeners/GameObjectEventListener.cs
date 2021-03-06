using Systems.Event.Scripts.Channels;
using UnityEngine;
using UnityEngine.Events;

namespace Systems.Event.Scripts.Listeners
{
    public class GameObjectEventListener : MonoBehaviour
    {
        [System.Serializable]
        private class GameObjectEvent
        {
            [SerializeField] private GameObjectEventChannelSo channel = default;
            [SerializeField] UnityEvent<GameObject> OnEventRaised;
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
            private void Respond(GameObject value) => OnEventRaised?.Invoke(value);
        }
        [SerializeField]
        private GameObjectEvent[] GameObjectEvents = new GameObjectEvent[] { };
        private void OnEnable()
        {
            foreach (var GameObjectEvent in GameObjectEvents)
                GameObjectEvent.Enable();
        }

        private void OnDisable()
        {
            foreach (var GameObjectEvent in GameObjectEvents)
                GameObjectEvent.Disable();
        }
    }
}