using UnityEngine;
using UnityEngine.Events;

namespace Systems.EventSystem.Scripts.Channels
{
    [CreateAssetMenu(menuName = "Events/Void Event Channel")]
    public class VoidEventChannelSO : ScriptableObject
    {
        public UnityAction OnEventRaised;
        public void RaiseEvent() => OnEventRaised?.Invoke();
    }
}
