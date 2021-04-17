using UnityEngine;
using UnityEngine.Events;

namespace Systems.Event.Scripts.Channels
{
    [CreateAssetMenu(menuName = "Events/String Event Channel")]
    public class StringEventChannelSo : ScriptableObject
    {
        public UnityAction<string> OnEventRaised;
        public void RaiseEvent(string value) => OnEventRaised?.Invoke(value);
    }
}
