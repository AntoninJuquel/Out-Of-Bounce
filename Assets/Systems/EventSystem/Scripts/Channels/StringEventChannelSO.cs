using UnityEngine;
using UnityEngine.Events;

namespace Systems.EventSystem.Scripts.Channels
{
    [CreateAssetMenu(menuName = "Events/String Event Channel")]
    public class StringEventChannelSO : ScriptableObject
    {
        public UnityAction<string> OnEventRaised;
        public void RaiseEvent(string value) => OnEventRaised?.Invoke(value);
    }
}
