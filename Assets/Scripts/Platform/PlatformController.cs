using System;
using System.Linq;
using Systems.Audio;
using Game;
using UnityEngine;
using Upgrade;

namespace Platform
{
    public class PlatformController : MonoBehaviour, ICollide
    {
        private UpgradeController _upgradeController;
        private LineRenderer _lineRenderer;
        private EdgeCollider2D _edgeCollider;
        private static readonly int Color = Shader.PropertyToID("_Color");
        private float Length => Vector2.Distance(_lineRenderer.GetPosition(0), _lineRenderer.GetPosition(1));

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _edgeCollider = GetComponent<EdgeCollider2D>();
            _upgradeController = GetComponent<UpgradeController>();
        }

        private void Disable()
        {
            CancelInvoke();
            gameObject.SetActive(false);
        }

        public void Activate(LineRenderer lineRenderer, float time)
        {
            var positions = new Vector3[2];
            lineRenderer.GetPositions(positions);

            _lineRenderer.positionCount = 2;
            _lineRenderer.material = lineRenderer.material;
            _lineRenderer.material.SetColor(Color, _lineRenderer.material.color * 2f);
            _lineRenderer.SetPositions(positions);
            _lineRenderer.widthMultiplier = _edgeCollider.edgeRadius = lineRenderer.widthMultiplier;
            _edgeCollider.SetPoints(Utilities.ToVector2Array(positions).ToList());

            AudioManager.Instance.Play("platform_activate");
            Invoke(nameof(Disable), time);
            _upgradeController.TriggerUpgrades();
        }

        public void Bounce(GameObject ball, float bouncyness)
        {
            var rigid = ball.GetComponent<Rigidbody2D>();
            rigid.velocity = rigid.velocity.normalized * (bouncyness * (2 / Mathf.Max(1, Length) + 1));
            AudioManager.Instance.Play("platform_bounce");
            Disable();
        }
    }
}