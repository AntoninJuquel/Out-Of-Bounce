using UnityEngine;
using UnityEngine.Events;

public class RendererEvents : MonoBehaviour
{
    [SerializeField] private UnityEvent onBecomeInvisible, onBecomeVisible;
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        if (_renderer.isVisible)
        {
            onBecomeVisible?.Invoke();
        }
        else
        {
            onBecomeInvisible?.Invoke();
        }
    }

    private void OnBecameInvisible()
    {
        onBecomeInvisible?.Invoke();
    }

    private void OnBecameVisible()
    {
        onBecomeVisible?.Invoke();
    }
}