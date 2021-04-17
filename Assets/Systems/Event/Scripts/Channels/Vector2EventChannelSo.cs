using UnityEngine;
using UnityEngine.Events;

namespace Systems.Event.Scripts.Channels
{
    [CreateAssetMenu(menuName = "Events/Vector2 Event Channel")]
    public class Vector2EventChannelSo : ScriptableObject
    {
        public UnityAction<Vector2> OnEventRaised;
        public void RaiseEvent(Vector2 value) => OnEventRaised?.Invoke(value);
    }
}
