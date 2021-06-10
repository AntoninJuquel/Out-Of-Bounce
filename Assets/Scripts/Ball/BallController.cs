using System.Collections;
using Game;
using UnityEngine;
using Upgrade;

namespace Ball
{
    public class BallController : MonoBehaviour
    {
        [SerializeField] private float duration = 10f, minStretch = .5f, stretchMult = 1 / 50f, stretchWhenSquash = .5f, stretchSpeed = 1f, squashAmount = .5f, squashSpeed = 1f;
        [SerializeField] private Transform render;
        [SerializeField] private GameObject deathParticles;
        private Rigidbody2D _rigidbody;
        private Transform _transform;
        private float _stretchAmount, _squashAmount, _stretchVel, _squashVel, _bouncyness = 15f;
        private BallManager _ballManager;
        private UpgradeController _upgradeController;
        private TrailRenderer _trailRenderer;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _transform = transform;
            _upgradeController = GetComponent<UpgradeController>();
            _trailRenderer = GetComponentInChildren<TrailRenderer>();
        }

        private void Start()
        {
            _ballManager = BallManager.Instance;
        }

        private void LateUpdate()
        {
            var velocity = _rigidbody.velocity;
            var targetStretch = Mathf.Clamp(1 - velocity.magnitude * stretchMult, minStretch, 1f);
            _stretchAmount = Mathf.SmoothDamp(_stretchAmount, targetStretch, ref _stretchVel, Time.deltaTime * stretchSpeed);
            _squashAmount = Mathf.SmoothDamp(_squashAmount, 1f, ref _squashVel, Time.deltaTime * squashSpeed);
            render.right = velocity.normalized;
            render.localScale = new Vector3(_squashAmount, _stretchAmount, 1);
            _trailRenderer.startWidth = render.localScale.y;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            _squashAmount = squashAmount;
            _stretchAmount = stretchWhenSquash;
            other.gameObject.TryGetComponent(out ICollide collide);
            collide?.Bounce(gameObject, _bouncyness);
            _upgradeController.OnBounce(other.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            other.gameObject.TryGetComponent(out ICollide collide);
            collide?.Bounce(gameObject, _bouncyness);
            _upgradeController.OnBounce(other.gameObject);
        }

        private IEnumerator DeathRoutine()
        {
            yield return new WaitUntil(() => _transform.position.y <= GameManager.Instance.GetDeathPosition());
            Destroy();
        }

        private void OnDisable()
        {
            Instantiate(deathParticles, transform.position, Quaternion.identity);
            StopAllCoroutines();
            CancelInvoke();
            if (_ballManager)
                _ballManager.RemoveBall(gameObject);
        }

        public void Setup(float bouncyness)
        {
            _rigidbody.simulated = GameManager.GameStatus != GameStatus.Starting;
            _transform.localScale = Vector3.one;
            _bouncyness = bouncyness;
            StartCoroutine(DeathRoutine());
            Invoke("Destroy", duration);
            if (_transform.position.y <= 0f)
                Destroy();
        }

        public void CancelTimedRoutine() => CancelInvoke();

        public void Destroy()
        {
            gameObject.SetActive(false);
        }
    }
}