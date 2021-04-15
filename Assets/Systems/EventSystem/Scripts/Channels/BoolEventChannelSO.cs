using UnityEngine;
using UnityEngine.Events;

namespace Systems.EventSystem.Scripts.Channels
{
    [CreateAssetMenu(menuName = "Events/Bool Event Channel")]
    public class BoolEventChannelSO : ScriptableObject
    {
        public UnityAction<bool> OnEventRaised;
        public void RaiseEvent(bool value) => OnEventRaised?.Invoke(value);
    }
}
