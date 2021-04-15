using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Events/Vector2 Event Channel")]
public class Vector2EventChannelSO : ScriptableObject
{
    public UnityAction<Vector2> OnEventRaised;
    public void RaiseEvent(Vector2 value) => OnEventRaised?.Invoke(value);
}
