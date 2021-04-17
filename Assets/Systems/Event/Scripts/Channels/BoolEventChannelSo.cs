using UnityEngine;
using UnityEngine.Events;

namespace Systems.Event.Scripts.Channels
{
    [CreateAssetMenu(menuName = "Events/Bool Event Channel")]
    public class BoolEventChannelSo : ScriptableObject
    {
        public UnityAction<bool> OnEventRaised;
        public void RaiseEvent(bool value) => OnEventRaised?.Invoke(value);
    }
}
