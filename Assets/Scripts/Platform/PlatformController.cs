using System.Linq;
using Balls;
using Utilities;
using MoreMountains.Feedbacks;
using UnityEngine;
using PowerUp;

namespace Platform
{
    public class PlatformController : MonoBehaviour, IBounceBall
    {
        [SerializeField] private MMF_Player bounceFeedback;
        private LineRenderer _lineRenderer;
        private EdgeCollider2D _edgeCollider;
        private PowerUpController _powerUpController;
        private static readonly int Color = Shader.PropertyToID("_Color");
        private float Length => Vector2.Distance(_lineRenderer.GetPosition(0), _lineRenderer.GetPosition(1));

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _edgeCollider = GetComponent<EdgeCollider2D>();
            _powerUpController = GetComponent<PowerUpController>();
        }

        private void OnDisable()
        {
            CancelInvoke();
        }

        private void Disable()
        {
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
            _edgeCollider.SetPoints(Vector2Utilities.ToVector2Array(positions).ToList());

            _powerUpController.TriggerUpgrades();
            Invoke(nameof(Disable), time);
        }

        public void Bounce(BallController ball, float bouncyness)
        {
            CancelInvoke();
            var rigid = ball.GetComponent<Rigidbody2D>();
            rigid.velocity = rigid.velocity.normalized * (bouncyness * (2 / Mathf.Max(1, Length) + 1));
            bounceFeedback.PlayFeedbacks();
        }
    }
}