using UnityEngine;

namespace Balls
{
    public class BallRenderer : MonoBehaviour
    {
        [SerializeField] private GameObject deathFeedback;
        [SerializeField] private Transform render, squashStretch;
        [SerializeField] private TrailRenderer trailRenderer;
        [SerializeField] private Rigidbody2D rigid;

        [SerializeField] private float
            minStretch = .5f,
            stretchMult = 1 / 50f,
            stretchSpeed = 1f,
            squashSpeed = 1f,
            trailTime = 1f;

        private float _stretchAmount, _squashAmount, _stretchVel, _squashVel;

        private void OnEnable()
        {
            trailRenderer.time = trailTime;
        }

        private void OnDisable()
        {
            trailRenderer.time = 0;
        }

        private void LateUpdate()
        {
            var velocity = rigid.velocity;
            var targetStretch = Mathf.Clamp(1 - velocity.magnitude * stretchMult, minStretch, 1f);
            _stretchAmount = Mathf.SmoothDamp(_stretchAmount, targetStretch, ref _stretchVel,
                Time.deltaTime * stretchSpeed);
            _squashAmount = Mathf.SmoothDamp(_squashAmount, 1f, ref _squashVel, Time.deltaTime * squashSpeed);
            render.right = velocity.normalized;
            render.localScale = new Vector3(_squashAmount, _stretchAmount, 1);
            trailRenderer.startWidth = squashStretch.localScale.y * render.localScale.y;
        }

        public void Die()
        {
            Instantiate(deathFeedback, transform.position, Quaternion.identity);
        }
    }
}