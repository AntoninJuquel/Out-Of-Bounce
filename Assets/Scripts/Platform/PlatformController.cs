using System;
using Systems.Audio;
using Game;
using UnityEngine;

namespace Platform
{
    public class PlatformController : MonoBehaviour, ICollide
    {
        private LineRenderer _lineRenderer;
        private EdgeCollider2D _edgeCollider;
        private static readonly int Color = Shader.PropertyToID("_Color");
        private float Length => Vector2.Distance(_lineRenderer.GetPosition(0), _lineRenderer.GetPosition(1));

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _edgeCollider = GetComponent<EdgeCollider2D>();
        }

        private void Disable()
        {
            CancelInvoke();
            _edgeCollider.enabled = false;
            gameObject.SetActive(false);
        }

        public void Activate(float time)
        {
            _lineRenderer.material.SetColor(Color, _lineRenderer.material.color * 2f);
            AudioManager.Instance.Play("platform_activate");
            _edgeCollider.enabled = true;
            Invoke(nameof(Disable), time);
        }

        public void Bounce(GameObject ball, float bouncyness)
        {
            var rigid = ball.GetComponent<Rigidbody2D>();
            rigid.velocity = rigid.velocity.normalized * (bouncyness * (2 / Mathf.Max(1, Length) + 1));
            AudioManager.Instance.Play("platform_bounce");
            Disable();
        }

        public LineRenderer GetLineRenderer() => _lineRenderer;
        public EdgeCollider2D GetEdgeCollider2D() => _edgeCollider;
        public void SetActive(bool active) => gameObject.SetActive(active);
    }
}