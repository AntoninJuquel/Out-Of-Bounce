using UnityEngine;
using UnityEngine.Events;

namespace Systems.Event.Scripts.Channels
{
    [CreateAssetMenu(menuName = "Events/Color Event Channel")]
    public class ColorEventChannelSo : ScriptableObject
    {
        public UnityAction<Color> OnEventRaised;
        public void RaiseEvent(Color value) => OnEventRaised?.Invoke(value);
    }
}
