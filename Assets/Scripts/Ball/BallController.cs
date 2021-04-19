using System;
using System.Collections;
using UnityEngine;

namespace Ball
{
    public class BallController : MonoBehaviour
    {
        [SerializeField] private float bouncyness = 15f,minStretch = .5f, stretchMult = 1 / 50f, stretchWhenSquash = .5f, stretchSpeed = 1f, squashAmount = .5f, squashSpeed = 1f;
        [SerializeField] private Transform render;
        private Rigidbody2D _rigidbody;
        private Transform _transform;
        private float _stretchAmount, _squashAmount, _stretchVel, _squashVel;
        private BallManager _ballManager;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _transform = transform;
        }

        private void Start()
        {
            _ballManager = BallManager.Instance;
            StartCoroutine(DeathRoutine());
        }

        private void LateUpdate()
        {
            var velocity = _rigidbody.velocity;
            var targetStretch = Mathf.Clamp(1 - velocity.magnitude * stretchMult, minStretch, 1f);
            _stretchAmount = Mathf.SmoothDamp(_stretchAmount, targetStretch, ref _stretchVel, Time.deltaTime * stretchSpeed);
            _squashAmount = Mathf.SmoothDamp(_squashAmount, 1f, ref _squashVel, Time.deltaTime * squashSpeed);
            render.right = velocity.normalized;
            render.localScale = new Vector3(_squashAmount, _stretchAmount, 1);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            _squashAmount = squashAmount;
            _stretchAmount = stretchWhenSquash;
            other.gameObject.TryGetComponent(out ICollide collide);
            collide?.Bounce(gameObject, bouncyness);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            other.gameObject.TryGetComponent(out ICollide collide);
            collide?.Bounce(gameObject, bouncyness);
        }

        private IEnumerator DeathRoutine()
        {
            yield return new WaitUntil(() => _transform.position.y < 0);
            Destroy();
        }

        private void OnDisable()
        {
            if (_ballManager)
                _ballManager.RemoveBall(gameObject);
        }

        public void Setup()
        {
            _rigidbody.simulated = GameManager.GameStatus != GameStatus.Starting;
            _transform.localScale = Vector3.one;
        }

        public void Destroy()
        {
            gameObject.SetActive(false);
            // I am dead ball manager
        }
    }
}