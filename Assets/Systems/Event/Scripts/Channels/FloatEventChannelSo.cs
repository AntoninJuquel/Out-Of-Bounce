using UnityEngine;
using UnityEngine.Events;

namespace Systems.Event.Scripts.Channels
{
    [CreateAssetMenu(menuName = "Events/Float Event Channel")]
    public class FloatEventChannelSo : ScriptableObject
    {
        public UnityAction<float> OnEventRaised;
        public void RaiseEvent(float value) => OnEventRaised?.Invoke(value);
    }
}
