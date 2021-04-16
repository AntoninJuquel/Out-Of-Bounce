using UnityEngine;

namespace Ball
{
    public class BallController : MonoBehaviour
    {
        [SerializeField] private float minStretch = .5f, stretchMult = 1 / 50f, stretchWhenSquash = .5f, stretchSpeed = 1f, squashAmount = .5f, squashSpeed = 1f;
        [SerializeField] private Transform render;
        private Rigidbody2D _rigidbody;
        private Transform _transform;
        private float _stretchAmount, _squashAmount, _stretchVel, _squashVel;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _transform = transform;
        }

        private void Update()
        {
            var velocity = _rigidbody.velocity;
            _transform.right = velocity.normalized;
            var targetStretch = Mathf.Clamp(1 - velocity.magnitude * stretchMult, minStretch, 1f);
            _stretchAmount = Mathf.SmoothDamp(_stretchAmount, targetStretch, ref _stretchVel, Time.deltaTime * stretchSpeed);
            _squashAmount = Mathf.SmoothDamp(_squashAmount, 1f, ref _squashVel, Time.deltaTime * squashSpeed);
            render.localScale = new Vector3(_squashAmount, _stretchAmount, 1);
        }

        public void Setup()
        {
            _rigidbody.simulated = GameManager.GameStatus != GameStatus.Starting;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            _squashAmount = squashAmount;
            _stretchAmount = stretchWhenSquash;
            other.gameObject.TryGetComponent(out ICollide collide);
            collide?.Bounce(gameObject, 15);
        }
    }
}