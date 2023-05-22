using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Balls
{
    public class BallController : MonoBehaviour
    {
        [SerializeField] private float duration = 10f;
        [SerializeField] private UnityEvent onDeath;
        private Rigidbody2D _rigidbody;
        private Transform _transform;
        private BallManager _ballManager;

        [field: SerializeField] public BallCamera BallCamera { get; private set; }
        [field: SerializeField] public BallRenderer BallRenderer { get; private set; }
        [field: SerializeField] public BallBounce BallBounce { get; private set; }


        private void Awake()
        {
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            _ballManager = BallManager.Instance;
        }

        private void OnDisable()
        {
            CleanUp();
        }

        private void OnDestroy()
        {
            CleanUp();
        }

        private void CleanUp()
        {
            DOTween.Kill(gameObject);
            DOTween.Kill(_transform);
            StopAllCoroutines();
            CancelInvoke();
        }

        [Button]
        public void Die()
        {
            if (_ballManager)
            {
                _ballManager.RemoveBall(this);
            }

            onDeath?.Invoke();
            gameObject.SetActive(false);
        }

        private IEnumerator DeathRoutine()
        {
            yield return new WaitUntil(() => _transform.position.y <= 0); // GameManager.Instance.GetDeathPosition();
            Die();
        }

        public void Setup(float bouncyness, int priority)
        {
            // _rigidbody.simulated = GameManager.GameStatus != GameStatus.Starting;
            _transform.localScale = Vector3.one;
            BallBounce.SetBouncyness(bouncyness);
            BallCamera.SetPriority(priority);
            StartCoroutine(DeathRoutine());
        }

        public void OnBecomeInvisible()
        {
            Invoke(nameof(Die), duration);
        }

        public void OnBecomeVisible()
        {
            CancelInvoke();
        }
    }
}