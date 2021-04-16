using System;
using UnityEngine;

namespace Platform
{
    public class PlatformController : MonoBehaviour, ICollide
    {
        private LineRenderer _lineRenderer;
        private EdgeCollider2D _edgeCollider;
        private float Length => Vector2.Distance(_lineRenderer.GetPosition(0), _lineRenderer.GetPosition(1));

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _edgeCollider = GetComponent<EdgeCollider2D>();
        }

        private void Disable()
        {
            //soundFXChannel.RaiseEvent("PlatformActivate");
            _edgeCollider.enabled = false;
            gameObject.SetActive(false);
        }

        public void Activate(float time)
        {
            //soundFXChannel.RaiseEvent("PlatformActivate");
            _edgeCollider.enabled = true;
            Invoke("Disable", time);
        }

        public void Bounce(GameObject ball, float bouncyness)
        {
            var rigid = ball.GetComponent<Rigidbody2D>();
            rigid.velocity = rigid.velocity.normalized * (bouncyness * (1 + 1 / Length));
        }

        public LineRenderer GetLineRenderer() => _lineRenderer;
        public EdgeCollider2D GetEdgeCollider2D() => _edgeCollider;
        public void SetActive(bool active) => gameObject.SetActive(active);
    }
}