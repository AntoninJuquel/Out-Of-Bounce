using UnityEngine;
using UnityEngine.Events;

namespace Systems.Event.Scripts.Channels
{
    [CreateAssetMenu(menuName = "Events/GameObject Event Channel")]
    public class GameObjectEventChannelSo : ScriptableObject
    {
        public UnityAction<GameObject> OnEventRaised;
        public void RaiseEvent(GameObject value) => OnEventRaised?.Invoke(value);
    }
}
