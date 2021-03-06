using UnityEngine;
using UnityEngine.Events;

namespace Systems.Event.Scripts.Channels
{
    [CreateAssetMenu(menuName = "Events/Int Event Channel")]
    public class IntEventChannelSo : ScriptableObject
    {
        public UnityAction<int> OnEventRaised;
        public void RaiseEvent(int value) => OnEventRaised?.Invoke(value);
    }
}
