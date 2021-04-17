using UnityEngine;
using UnityEngine.Events;

namespace Systems.Event.Scripts.Channels
{
    [CreateAssetMenu(menuName = "Events/Void Event Channel")]
    public class VoidEventChannelSo : ScriptableObject
    {
        public UnityAction OnEventRaised;
        public void RaiseEvent() => OnEventRaised?.Invoke();
    }
}
